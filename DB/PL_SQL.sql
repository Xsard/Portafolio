set serveroutput on 
/
CREATE OR REPLACE FUNCTION GENERAR_CON(p_email VARCHAR2, p_psw VARCHAR2)
    RETURN VARCHAR2
    IS
    BEGIN
        RETURN DBMS_CRYPTO.HASH(UTL_RAW.CAST_TO_RAW(UPPER(p_email) ||''|| UPPER(p_psw)),DBMS_CRYPTO.HASH_SH1);
    END;
/
CREATE OR REPLACE PACKAGE login_desk AS
    PROCEDURE AUTENTIFICAR(email_aut IN USUARIO.email%type, psw_aut IN USUARIO.contraseña%type, usr_con OUT SYS_REFCURSOR);
END login_desk;
/

CREATE OR REPLACE PACKAGE BODY login_desk AS
    PROCEDURE AUTENTIFICAR(email_aut IN USUARIO.email%type, psw_aut IN USUARIO.contraseña%type, usr_con OUT SYS_REFCURSOR)
    IS
    v_count number;
    user_not_found EXCEPTION;
    v_pass VARCHAR2(40);
    PRAGMA EXCEPTION_INIT(user_not_found, -20000);
    BEGIN 
        v_pass:= GENERAR_CON(email_aut, psw_aut);
        SELECT COUNT(usr.id_usuario) 
            INTO V_COUNT 
        FROM ADMINISTRADOR ADM JOIN USUARIO USR ON(adm.id_usuario = usr.id_usuario)
            WHERE usr.email = email_aut and usr.contraseña = v_pass;
        IF V_COUNT = 1 THEN
            OPEN usr_con FOR
                SELECT adm.nombres_admin||' '||adm.apellidos_admin as nombre, 'Administrador' as rol 
                FROM ADMINISTRADOR ADM JOIN USUARIO USR ON(adm.id_usuario = usr.id_usuario)
                    WHERE usr.email = email_aut and usr.contraseña = v_pass;
        ELSE 
            SELECT COUNT(usr.id_usuario)
                INTO V_COUNT
            FROM FUNCIONARIO FUN JOIN USUARIO USR ON(fun.id_usuario = usr.id_usuario) 
                WHERE usr.email = email_aut and usr.contraseña = v_pass;
            IF V_COUNT = 1 THEN
            OPEN usr_con FOR
                SELECT fun.nombres_funcionario||' '||fun.apellidos_funcionario AS nombre, 'Funcionario' AS rol 
                FROM FUNCIONARIO FUN JOIN USUARIO USR ON(fun.id_usuario = usr.id_usuario) 
                    WHERE usr.email = email_aut and usr.contraseña = v_pass;   
            ELSE
                RAISE user_not_found;
            END IF;
        END IF;
    EXCEPTION 
        WHEN user_not_found THEN
            DBMS_OUTPUT.PUT_LINE('EL USUARIO NO EXISTE');
            usr_con:= NULL;
    END;
END login_desk;
/
CREATE OR REPLACE PACKAGE Mantener_Dpto
    AS
    PROCEDURE insertar_dpto(tarifa IN DEPARTAMENTO.TARIFA_DIARIA%TYPE, DIREC IN DEPARTAMENTO.DIRECCION%TYPE, 
        NRO IN DEPARTAMENTO.NRO_DPTO%TYPE, CAP IN DEPARTAMENTO.CAPACIDAD%TYPE, COMUNA IN DEPARTAMENTO.ID_COMUNA%TYPE, R OUT INTEGER);
    PROCEDURE actualizar_dpto(identificador IN DEPARTAMENTO.ID_DPTO%TYPE,tarifa IN DEPARTAMENTO.TARIFA_DIARIA%TYPE, DIREC IN DEPARTAMENTO.DIRECCION%TYPE, 
        NRO IN DEPARTAMENTO.NRO_DPTO%TYPE, CAP IN DEPARTAMENTO.CAPACIDAD%TYPE, COMUNA IN DEPARTAMENTO.ID_COMUNA%TYPE, R OUT INTEGER);
    PROCEDURE eliminar_dpto(identificador IN DEPARTAMENTO.ID_DPTO%TYPE, R OUT INTEGER);
    PROCEDURE listar_dpto(Deptos OUT SYS_REFCURSOR);
END Mantener_Dpto;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Dpto
    AS
    PROCEDURE insertar_dpto(tarifa IN DEPARTAMENTO.TARIFA_DIARIA%TYPE, DIREC IN DEPARTAMENTO.DIRECCION%TYPE, 
        NRO IN DEPARTAMENTO.NRO_DPTO%TYPE, CAP IN DEPARTAMENTO.CAPACIDAD%TYPE, COMUNA IN DEPARTAMENTO.ID_COMUNA%TYPE, R OUT INTEGER)
    IS 
        id_col rowid;
    BEGIN
        INSERT INTO DEPARTAMENTO(TARIFA_DIARIA, DIRECCION, NRO_DPTO, CAPACIDAD, ID_COMUNA) VALUES(tarifa, direc, nro, cap, COMUNA) RETURNING rowid INTO id_col;
        IF id_col IS NOT NULL THEN
            r:=1;
            COMMIT;
        END IF;
    END;
    
    PROCEDURE actualizar_dpto(identificador IN DEPARTAMENTO.ID_DPTO%TYPE,tarifa IN DEPARTAMENTO.TARIFA_DIARIA%TYPE, DIREC IN DEPARTAMENTO.DIRECCION%TYPE, 
        NRO IN DEPARTAMENTO.NRO_DPTO%TYPE, CAP IN DEPARTAMENTO.CAPACIDAD%TYPE, COMUNA IN DEPARTAMENTO.ID_COMUNA%TYPE, R OUT INTEGER)
    IS 
    BEGIN
        UPDATE DEPARTAMENTO 
            SET TARIFA_DIARIA = tarifa, DIRECCION = direc, NRO_DPTO = nro,CAPACIDAD = cap, ID_COMUNA = comuna
        WHERE ID_DPTO =  identificador RETURNING 1 INTO R;
        IF r = 1 THEN
            COMMIT;
        END IF;
    END;
    
    PROCEDURE eliminar_dpto(identificador IN DEPARTAMENTO.ID_DPTO%TYPE, R OUT INTEGER)
    IS 
    BEGIN 
        DELETE FROM DEPARTAMENTO WHERE ID_DPTO =  identificador RETURNING 1 INTO r;
        IF r = 1 THEN
            COMMIT;
        END IF;
    END;
    
    PROCEDURE listar_dpto(Deptos OUT SYS_REFCURSOR)
    IS
    BEGIN
        OPEN Deptos FOR
            SELECT * FROM DEPARTAMENTO DPTO JOIN COMUNA CM ON(cm.id_comuna=dpto.id_comuna);
    END;
END Mantener_Dpto;
/
CREATE OR REPLACE PACKAGE Ubicacion 
    AS
    PROCEDURE listar_comunas(Comunas out SYS_REFCURSOR);
END Ubicacion;
/
CREATE OR REPLACE PACKAGE BODY Ubicacion
    AS
    PROCEDURE listar_comunas(Comunas out SYS_REFCURSOR)
    IS
    BEGIN
        OPEN Comunas FOR
            SELECT * FROM COMUNA;
    END;
END Ubicacion;
/
CREATE OR REPLACE PACKAGE Mantener_Servicios_Extras
    AS
    PROCEDURE insertar_svextra(nombre IN SERVICIO_EXTRA.NOMBRE_SERV_EX%TYPE, descripcion IN SERVICIO_EXTRA.DESC_SERV_EX%TYPE, valor IN SERVICIO_EXTRA.VALOR_SERV_EX%TYPE, R OUT INTEGER);
    PROCEDURE actualizar_svextra(identificador IN SERVICIO_EXTRA.ID_SVC_EX%TYPE, nombre IN SERVICIO_EXTRA.NOMBRE_SERV_EX%TYPE,
        descripcion IN SERVICIO_EXTRA.DESC_SERV_EX%TYPE, valor IN SERVICIO_EXTRA.VALOR_SERV_EX%TYPE, R OUT INTEGER);
    PROCEDURE eliminar_svextra(identificador SERVICIO_EXTRA.ID_SVC_EX%TYPE, R OUT INTEGER);
    PROCEDURE listar_svextra(Servicios_Ex OUT SYS_REFCURSOR);

END Mantener_Servicios_Extras;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Servicios_Extras
    AS
    PROCEDURE insertar_svextra(nombre IN SERVICIO_EXTRA.NOMBRE_SERV_EX%TYPE, descripcion IN SERVICIO_EXTRA.DESC_SERV_EX%TYPE, valor IN SERVICIO_EXTRA.VALOR_SERV_EX%TYPE, R OUT INTEGER)
    IS
        id_col rowid;
    BEGIN
        INSERT INTO SERVICIO_EXTRA(NOMBRE_SERV_EX, DESC_SERV_EX, VALOR_SERV_EX) VALUES(nombre, descripcion, valor) RETURNING rowid INTO id_col;
        IF id_col IS NOT NULL THEN
            r:=1;
            COMMIT;
        END IF;
    END;
    PROCEDURE actualizar_svextra(identificador IN SERVICIO_EXTRA.ID_SVC_EX%TYPE, nombre IN SERVICIO_EXTRA.NOMBRE_SERV_EX%TYPE,
        descripcion IN SERVICIO_EXTRA.DESC_SERV_EX%TYPE, valor IN SERVICIO_EXTRA.VALOR_SERV_EX%TYPE, R OUT INTEGER)
    IS
    BEGIN
        UPDATE SERVICIO_EXTRA 
            SET NOMBRE_SERV_EX = nombre, DESC_SERV_EX = descripcion, VALOR_SERV_EX = valor
        WHERE ID_SVC_EX = identificador RETURNING 1 INTO R;
        IF r = 1 THEN
            COMMIT;
        END IF;
    END;
    PROCEDURE eliminar_svextra(identificador SERVICIO_EXTRA.ID_SVC_EX%TYPE, R OUT INTEGER)
    IS 
    BEGIN 
        DELETE FROM SERVICIO_EXTRA WHERE ID_SVC_EX =  identificador RETURNING 1 INTO r;
        IF r = 1 THEN
            COMMIT;
        END IF;
    END;
    PROCEDURE listar_svextra(Servicios_Ex OUT SYS_REFCURSOR)
    IS
    BEGIN
        OPEN Servicios_Ex FOR
            SELECT * FROM SERVICIO_EXTRA;
    END;
END Mantener_Servicios_Extras;
/
CREATE OR REPLACE PACKAGE Mantener_Inventario_Dpto
    AS
    PROCEDURE insertar_objeto(id_Dpto IN INVENTARIO_DPTO.ID_DPTO%TYPE, nombre IN INVENTARIO_DPTO.NOMBRE_OBJETO%TYPE,
        cantidad IN INVENTARIO_DPTO.CANT_OBJETO%TYPE, valor IN INVENTARIO_DPTO.VALOR_UNITARIO_OBJ%TYPE, R OUT INTEGER);
    PROCEDURE actualizar_objeto(identificador IN INVENTARIO_DPTO.ID_INV%TYPE, nombre IN INVENTARIO_DPTO.NOMBRE_OBJETO%TYPE,
        cantidad IN INVENTARIO_DPTO.CANT_OBJETO%TYPE, valor IN INVENTARIO_DPTO.VALOR_UNITARIO_OBJ%TYPE, R OUT INTEGER);
    PROCEDURE eliminar_objeto(identificador IN INVENTARIO_DPTO.ID_INV%TYPE, R OUT INTEGER);
    PROCEDURE listar_inventario(id_dp IN INVENTARIO_DPTO.ID_DPTO%TYPE, Inventario OUT SYS_REFCURSOR);
END Mantener_Inventario_Dpto;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Inventario_Dpto
    AS
    PROCEDURE insertar_objeto(id_Dpto IN INVENTARIO_DPTO.ID_DPTO%TYPE, nombre IN INVENTARIO_DPTO.NOMBRE_OBJETO%TYPE,
        cantidad IN INVENTARIO_DPTO.CANT_OBJETO%TYPE, valor IN INVENTARIO_DPTO.VALOR_UNITARIO_OBJ%TYPE, R OUT INTEGER)
    IS
        id_col rowid;
    BEGIN
        INSERT INTO INVENTARIO_DPTO(ID_DPTO, NOMBRE_OBJETO, CANT_OBJETO, VALOR_UNITARIO_OBJ) VALUES( id_Dpto, nombre, cantidad, valor) RETURNING rowid INTO id_col;
        IF id_col IS NOT NULL THEN
            r:=1;
            COMMIT;
        END IF;
    END;
    PROCEDURE actualizar_objeto(identificador IN INVENTARIO_DPTO.ID_INV%TYPE, nombre IN INVENTARIO_DPTO.NOMBRE_OBJETO%TYPE,
        cantidad IN INVENTARIO_DPTO.CANT_OBJETO%TYPE, valor IN INVENTARIO_DPTO.VALOR_UNITARIO_OBJ%TYPE, R OUT INTEGER)
        IS
    BEGIN
        UPDATE INVENTARIO_DPTO 
            SET NOMBRE_OBJETO = nombre, CANT_OBJETO = cantidad, VALOR_UNITARIO_OBJ = valor
        WHERE ID_INV = identificador RETURNING 1 INTO R;
        IF r = 1 THEN
            COMMIT;
        END IF;
    END;
    PROCEDURE eliminar_objeto(identificador IN INVENTARIO_DPTO.ID_INV%TYPE, R OUT INTEGER)
        IS 
    BEGIN 
        DELETE FROM INVENTARIO_DPTO WHERE ID_INV = identificador RETURNING 1 INTO r;
        IF r = 1 THEN
            COMMIT;
        END IF;
    END;
    PROCEDURE listar_inventario(id_dp IN INVENTARIO_DPTO.ID_DPTO%TYPE, Inventario OUT SYS_REFCURSOR)
        IS
    BEGIN
        OPEN Inventario FOR
            SELECT * FROM INVENTARIO_DPTO WHERE ID_DPTO =id_dp;
    END;
END Mantener_Inventario_Dpto;
/
CREATE OR REPLACE PACKAGE Mantener_Img AS
    PROCEDURE Agregar_Img(id_dp IN INVENTARIO_DPTO.ID_DPTO%TYPE, path_img IN OUT FOTOGRAFIA_DPTO.FOTO_PATH%TYPE, alt_img IN FOTOGRAFIA_DPTO.ALT_FOTO%TYPE, extension IN VARCHAR2, NEW_FILE OUT VARCHAR2, R OUT INTEGER);
END Mantener_Img;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Img AS
    PROCEDURE Agregar_Img(id_dp IN INVENTARIO_DPTO.ID_DPTO%TYPE, path_img IN OUT FOTOGRAFIA_DPTO.FOTO_PATH%TYPE, alt_img IN FOTOGRAFIA_DPTO.ALT_FOTO%TYPE, extension IN VARCHAR2, NEW_FILE OUT VARCHAR2, R OUT INTEGER)
    IS
    BEGIN 
        SAVEPOINT A;
        INSERT INTO FOTOGRAFIA_DPTO(ID_DPTO, ALT_FOTO) VALUES(id_dp, alt_img) RETURNING ID_FOTO INTO R;
        UPDATE FOTOGRAFIA_DPTO SET FOTO_PATH = path_img||r||extension WHERE ID_FOTO = R RETURNING 1, r||extension INTO r, NEW_FILE;
            IF r = 1 THEN
                COMMIT;
        END IF;
    END;
END Mantener_Img;
/
DECLARE
    v_pass VARCHAR2(40);
BEGIN
    v_pass:= GENERAR_CON('test@gmail.com', 'test123');
    INSERT INTO Usuario VALUES (1, 'test@gmail.com', v_pass, 123456789, null, null, null);
    INSERT INTO Administrador values(1, '20441631-1', 'Test', 'Iteración', 1);
    UPDATE Usuario SET id_admin = 1 WHERE id_usuario = 1;
    COMMIT;
END;