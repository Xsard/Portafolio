set serveroutput on 
/
CREATE OR REPLACE PACKAGE login_desk AS

    FUNCTION AUTENTIFICAR(email_aut USUARIO.email%type, psw_aut USUARIO.contraseña%type)
    RETURN SYS_REFCURSOR;
END login_desk;
/

CREATE OR REPLACE PACKAGE BODY login_desk AS
    FUNCTION AUTENTIFICAR(email_aut USUARIO.email%type, psw_aut USUARIO.contraseña%type)
    RETURN SYS_REFCURSOR
    IS
    usr_con SYS_REFCURSOR;
    v_count number;
    user_not_found EXCEPTION;
    PRAGMA EXCEPTION_INIT(user_not_found, -20000);
    BEGIN 
        SELECT COUNT(usr.id_usuario) 
            INTO V_COUNT 
        FROM ADMINISTRADOR ADM JOIN USUARIO USR ON(adm.id_usuario = usr.id_usuario)
            WHERE usr.email = email_aut and usr.contraseña = psw_aut;
        IF V_COUNT = 1 THEN
            OPEN usr_con FOR
                SELECT adm.nombres_admin||' '||adm.apellidos_admin as nombre, 'Administrador' as rol 
                FROM ADMINISTRADOR ADM JOIN USUARIO USR ON(adm.id_usuario = usr.id_usuario)
                    WHERE usr.email = email_aut and usr.contraseña = psw_aut;
        ELSE 
            SELECT COUNT(usr.id_usuario)
                INTO V_COUNT
            FROM FUNCIONARIO FUN JOIN USUARIO USR ON(fun.id_usuario = usr.id_usuario) 
                WHERE usr.email = email_aut and usr.contraseña = psw_aut;
            IF V_COUNT = 1 THEN
            OPEN usr_con FOR
                SELECT fun.nombres_funcionario||' '||fun.apellidos_funcionario AS nombre, 'Funcionario' AS rol 
                FROM FUNCIONARIO FUN JOIN USUARIO USR ON(fun.id_usuario = usr.id_usuario) 
                    WHERE usr.email = email_aut and usr.contraseña = psw_aut;   
            ELSE
                RAISE user_not_found;
            END IF;
        END IF;
        RETURN usr_con;
    EXCEPTION 
        WHEN user_not_found THEN
            DBMS_OUTPUT.PUT_LINE('EL USUARIO NO EXISTE');
            RETURN NULL;
    END;
END login_desk;
/
CREATE OR REPLACE PACKAGE Mantener_Dpto
    AS
    FUNCTION insertar_dpto(tarifa DEPARTAMENTO.TARIFA_DIARIA%TYPE, DIREC DEPARTAMENTO.DIRECCION%TYPE, NRO DEPARTAMENTO.NRO_DPTO%TYPE, CAP DEPARTAMENTO.CAPACIDAD%TYPE, COMUNA DEPARTAMENTO.ID_COMUNA%TYPE) 
    RETURN INTEGER;
    FUNCTION actualizar_dpto(identificador DEPARTAMENTO.ID_DPTO%TYPE,tarifa DEPARTAMENTO.TARIFA_DIARIA%TYPE, DIREC DEPARTAMENTO.DIRECCION%TYPE, NRO DEPARTAMENTO.NRO_DPTO%TYPE, CAP DEPARTAMENTO.CAPACIDAD%TYPE, COMUNA DEPARTAMENTO.ID_COMUNA%TYPE)
    RETURN INTEGER;
    FUNCTION eliminar_dpto(identificador DEPARTAMENTO.ID_DPTO%TYPE)
    RETURN INTEGER;
    FUNCTION listar_dpto
    RETURN SYS_REFCURSOR;
END Mantener_Dpto;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Dpto
    AS
    FUNCTION insertar_dpto(tarifa DEPARTAMENTO.TARIFA_DIARIA%TYPE, DIREC DEPARTAMENTO.DIRECCION%TYPE, NRO DEPARTAMENTO.NRO_DPTO%TYPE, CAP DEPARTAMENTO.CAPACIDAD%TYPE, COMUNA DEPARTAMENTO.ID_COMUNA%TYPE) 
    RETURN INTEGER
    IS 
        id_col rowid;
        r integer;
    BEGIN
        INSERT INTO DEPARTAMENTO(TARIFA_DIARIA, DIRECCION, NRO_DPTO, CAPACIDAD, ID_COMUNA) VALUES(tarifa, direc, nro, cap, COMUNA) RETURNING rowid INTO id_col;
        IF id_col IS NOT NULL THEN
            r:=1;
            COMMIT;
        END IF;
        RETURN r;
    END;
    
    FUNCTION  actualizar_dpto(identificador DEPARTAMENTO.ID_DPTO%TYPE,tarifa DEPARTAMENTO.TARIFA_DIARIA%TYPE, DIREC DEPARTAMENTO.DIRECCION%TYPE, NRO DEPARTAMENTO.NRO_DPTO%TYPE, CAP DEPARTAMENTO.CAPACIDAD%TYPE, COMUNA DEPARTAMENTO.ID_COMUNA%TYPE)
    RETURN INTEGER
    IS 
        r integer; 
    BEGIN
        UPDATE DEPARTAMENTO 
            SET TARIFA_DIARIA = tarifa, DIRECCION = direc, NRO_DPTO = nro,CAPACIDAD = cap, ID_COMUNA = comuna
        WHERE ID_DPTO =  identificador RETURNING 1 INTO R;
        IF r = 1 THEN
            COMMIT;
        END IF;
        RETURN r;
    END;
    
    FUNCTION eliminar_dpto(identificador DEPARTAMENTO.ID_DPTO%TYPE)
    RETURN INTEGER
    IS 
        r integer;
    BEGIN 
        DELETE FROM DEPARTAMENTO WHERE ID_DPTO =  identificador RETURNING 1 INTO r;
        IF r = 1 THEN
            COMMIT;
        END IF;
        RETURN R;
    END;
    
    FUNCTION listar_dpto
    RETURN SYS_REFCURSOR
    IS
        Deptos SYS_REFCURSOR;
    BEGIN
        OPEN Deptos FOR
            SELECT * FROM DEPARTAMENTO DPTO JOIN COMUNA CM ON(cm.id_comuna=dpto.id_comuna);
        RETURN Deptos;
    END;
END Mantener_Dpto;
/
CREATE OR REPLACE PACKAGE Ubicacion 
    AS
    FUNCTION listar_comunas
    RETURN SYS_REFCURSOR;
END Ubicacion;
/
CREATE OR REPLACE PACKAGE BODY Ubicacion
    AS
    FUNCTION listar_comunas RETURN SYS_REFCURSOR
    IS
        Comunas SYS_REFCURSOR;
    BEGIN
        OPEN Comunas FOR
            SELECT * FROM COMUNA;
        RETURN Comunas;
    END;
END Ubicacion;
/
CREATE OR REPLACE PACKAGE Mantener_Servicios_Extras
    AS
    FUNCTION insertar_svextra(nombre SERVICIO_EXTRA.NOMBRE_SERV_EX%TYPE, descripcion SERVICIO_EXTRA.DESC_SERV_EX%TYPE, valor SERVICIO_EXTRA.VALOR_SERV_EX%TYPE) 
    RETURN INTEGER;
    FUNCTION actualizar_svextra(identificador SERVICIO_EXTRA.ID_SVC_EX%TYPE, nombre SERVICIO_EXTRA.NOMBRE_SERV_EX%TYPE, descripcion SERVICIO_EXTRA.DESC_SERV_EX%TYPE, valor SERVICIO_EXTRA.VALOR_SERV_EX%TYPE)
    RETURN INTEGER;
    FUNCTION eliminar_svextra(identificador SERVICIO_EXTRA.ID_SVC_EX%TYPE)
    RETURN INTEGER;
    FUNCTION listar_svextra
    RETURN SYS_REFCURSOR;
END Mantener_Servicios_Extras;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Servicios_Extras
    AS
    FUNCTION insertar_svextra(nombre SERVICIO_EXTRA.NOMBRE_SERV_EX%TYPE, descripcion SERVICIO_EXTRA.DESC_SERV_EX%TYPE, valor SERVICIO_EXTRA.VALOR_SERV_EX%TYPE) 
    RETURN INTEGER
    IS
        id_col rowid;
        r integer;
    BEGIN
        INSERT INTO SERVICIO_EXTRA(NOMBRE_SERV_EX, DESC_SERV_EX, VALOR_SERV_EX) VALUES(nombre, descripcion, valor) RETURNING rowid INTO id_col;
        IF id_col IS NOT NULL THEN
            r:=1;
            COMMIT;
        END IF;
        RETURN r;
    END;
    FUNCTION actualizar_svextra(identificador SERVICIO_EXTRA.ID_SVC_EX%TYPE, nombre SERVICIO_EXTRA.NOMBRE_SERV_EX%TYPE, descripcion SERVICIO_EXTRA.DESC_SERV_EX%TYPE, valor SERVICIO_EXTRA.VALOR_SERV_EX%TYPE)
    RETURN INTEGER
    IS
        r integer; 
    BEGIN
        UPDATE SERVICIO_EXTRA 
            SET NOMBRE_SERV_EX = nombre, DESC_SERV_EX = descripcion, VALOR_SERV_EX = valor
        WHERE ID_SVC_EX = identificador RETURNING 1 INTO R;
        IF r = 1 THEN
            COMMIT;
        END IF;
        RETURN r;
    END;
    FUNCTION eliminar_svextra(identificador SERVICIO_EXTRA.ID_SVC_EX%TYPE)
    RETURN INTEGER
    IS 
        r integer;
    BEGIN 
        DELETE FROM SERVICIO_EXTRA WHERE ID_SVC_EX =  identificador RETURNING 1 INTO r;
        IF r = 1 THEN
            COMMIT;
        END IF;
        RETURN R;
    END;
    FUNCTION listar_svextra
        RETURN SYS_REFCURSOR
    IS
        Servicios_Ex SYS_REFCURSOR;
    BEGIN
        OPEN Servicios_Ex FOR
            SELECT * FROM SERVICIO_EXTRA;
        RETURN Servicios_Ex;
    END;
END Mantener_Servicios_Extras;
/
CREATE OR REPLACE PACKAGE Mantener_Inventario_Dpto
    AS
    FUNCTION insertar_objeto(id_Dpto INVENTARIO_DPTO.ID_DPTO%TYPE, nombre INVENTARIO_DPTO.NOMBRE_OBJETO%TYPE, cantidad INVENTARIO_DPTO.CANT_OBJETO%TYPE, valor INVENTARIO_DPTO.VALOR_UNITARIO_OBJ%TYPE) 
    RETURN INTEGER;
    FUNCTION actualizar_objeto(identificador INVENTARIO_DPTO.ID_INV%TYPE, nombre INVENTARIO_DPTO.NOMBRE_OBJETO%TYPE, cantidad INVENTARIO_DPTO.CANT_OBJETO%TYPE, valor INVENTARIO_DPTO.VALOR_UNITARIO_OBJ%TYPE)
    RETURN INTEGER;
    FUNCTION eliminar_objeto(identificador INVENTARIO_DPTO.ID_INV%TYPE)
    RETURN INTEGER;
    FUNCTION listar_inventario(id_dp INVENTARIO_DPTO.ID_DPTO%TYPE)
    RETURN SYS_REFCURSOR;
END Mantener_Inventario_Dpto;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Inventario_Dpto
    AS
    FUNCTION insertar_objeto(id_Dpto INVENTARIO_DPTO.ID_DPTO%TYPE, nombre INVENTARIO_DPTO.NOMBRE_OBJETO%TYPE, cantidad INVENTARIO_DPTO.CANT_OBJETO%TYPE, valor INVENTARIO_DPTO.VALOR_UNITARIO_OBJ%TYPE) 
    RETURN INTEGER
    IS
        id_col rowid;
        r integer;
    BEGIN
        INSERT INTO INVENTARIO_DPTO(ID_DPTO, NOMBRE_OBJETO, CANT_OBJETO, VALOR_UNITARIO_OBJ) VALUES( id_Dpto, nombre, cantidad, valor) RETURNING rowid INTO id_col;
        IF id_col IS NOT NULL THEN
            r:=1;
            COMMIT;
        END IF;
        RETURN r;
    END;
    FUNCTION actualizar_objeto(identificador INVENTARIO_DPTO.ID_INV%TYPE, nombre INVENTARIO_DPTO.NOMBRE_OBJETO%TYPE, cantidad INVENTARIO_DPTO.CANT_OBJETO%TYPE, valor INVENTARIO_DPTO.VALOR_UNITARIO_OBJ%TYPE)
    RETURN INTEGER
        IS
        r integer; 
    BEGIN
        UPDATE INVENTARIO_DPTO 
            SET NOMBRE_OBJETO = nombre, CANT_OBJETO = cantidad, VALOR_UNITARIO_OBJ = valor
        WHERE ID_INV = identificador RETURNING 1 INTO R;
        IF r = 1 THEN
            COMMIT;
        END IF;
        RETURN r;
    END;
    FUNCTION eliminar_objeto(identificador INVENTARIO_DPTO.ID_INV%TYPE)
    RETURN INTEGER
        IS 
        r integer;
    BEGIN 
        DELETE FROM INVENTARIO_DPTO WHERE ID_INV =  identificador RETURNING 1 INTO r;
        IF r = 1 THEN
            COMMIT;
        END IF;
        RETURN R;
    END;
    FUNCTION listar_inventario(id_dp INVENTARIO_DPTO.ID_DPTO%TYPE)
    RETURN SYS_REFCURSOR
        IS
        Inventario SYS_REFCURSOR;
    BEGIN
        OPEN Inventario FOR
            SELECT * FROM INVENTARIO_DPTO WHERE ID_DPTO =id_dp;
        RETURN Inventario;
    END;
END Mantener_Inventario_Dpto;