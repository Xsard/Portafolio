/*Mostrar mensajes por consola*/
set serveroutput on 
/
/*Cifrar la contraseña*/
CREATE OR REPLACE FUNCTION GENERAR_CON(p_email VARCHAR2, p_psw VARCHAR2)
    RETURN VARCHAR2
    IS
    BEGIN
        RETURN DBMS_CRYPTO.HASH(UTL_RAW.CAST_TO_RAW(UPPER(p_email) ||''|| UPPER(p_psw)),DBMS_CRYPTO.HASH_SH1);
    END;
/
/*Generar paquete de procedimientos relacionados al login*/
CREATE OR REPLACE PACKAGE login_desk AS
    PROCEDURE AUTENTIFICAR(email_aut IN USUARIO.email%type, psw_aut IN USUARIO.contraseña%type, usr_con OUT SYS_REFCURSOR);
END login_desk;
/
CREATE OR REPLACE PACKAGE BODY login_desk AS
    /*Validar los datos del usuario*/
    PROCEDURE AUTENTIFICAR(email_aut IN USUARIO.email%type, psw_aut IN USUARIO.contraseña%type, usr_con OUT SYS_REFCURSOR)
    IS
        v_count number;
        user_not_found EXCEPTION;
        v_pass VARCHAR2(40);
        PRAGMA EXCEPTION_INIT(user_not_found, -20000);
    BEGIN 
        /*Generar la contraseña*/
        v_pass:= GENERAR_CON(email_aut, psw_aut);
        
        /*Validar si el usuario es Administrador*/
        SELECT COUNT(usr.id_usuario) 
            INTO V_COUNT 
        FROM ADMINISTRADOR ADM JOIN USUARIO USR ON(adm.id_usuario = usr.id_usuario)
            WHERE usr.email = email_aut and usr.contraseña = v_pass;
        /*Retornar los datos del Administrador si el usuario lo es*/
        IF V_COUNT = 1 THEN
            OPEN usr_con FOR
                SELECT adm.nombres_admin||' '||adm.apellidos_admin as nombre, 'Administrador' as rol 
                FROM ADMINISTRADOR ADM JOIN USUARIO USR ON(adm.id_usuario = usr.id_usuario)
                    WHERE usr.email = email_aut and usr.contraseña = v_pass;
        ELSE 
            /*Consultar si el usuario es Funcionario*/
            SELECT COUNT(usr.id_usuario)
                INTO V_COUNT
            FROM FUNCIONARIO FUN JOIN USUARIO USR ON(fun.id_usuario = usr.id_usuario) 
                WHERE usr.email = email_aut and usr.contraseña = v_pass;
                
            /*Retornar los datos del Funcionario si el usuario lo es*/
            IF V_COUNT = 1 THEN
            OPEN usr_con FOR
                SELECT fun.nombres_funcionario||' '||fun.apellidos_funcionario AS nombre, 'Funcionario' AS rol 
                FROM FUNCIONARIO FUN JOIN USUARIO USR ON(fun.id_usuario = usr.id_usuario) 
                    WHERE usr.email = email_aut and usr.contraseña = v_pass;   
            ELSE
            
                /*En caso de no haber usuario con los datos entregados se inicia una excepción*/
                RAISE user_not_found;
            END IF;
        END IF;
    EXCEPTION 
        WHEN user_not_found THEN
            usr_con:= NULL;
    END;
END login_desk;
/
/*CRUD departamentos*/
CREATE OR REPLACE PACKAGE Mantener_Dpto
    AS
    PROCEDURE insertar_dpto(nombre IN DEPARTAMENTO.NOMBRE_DPTO%TYPE, tarifa IN DEPARTAMENTO.TARIFA_DIARIA%TYPE, DIREC IN DEPARTAMENTO.DIRECCION%TYPE, 
        NRO IN DEPARTAMENTO.NRO_DPTO%TYPE, CAP IN DEPARTAMENTO.CAPACIDAD%TYPE, COMUNA IN DEPARTAMENTO.ID_COMUNA%TYPE, disponibilidad IN DEPARTAMENTO.DISPONIBILIDAD%TYPE, R OUT INTEGER);
        
    PROCEDURE actualizar_dpto(identificador IN DEPARTAMENTO.ID_DPTO%TYPE, nombre IN DEPARTAMENTO.NOMBRE_DPTO%TYPE, tarifa IN DEPARTAMENTO.TARIFA_DIARIA%TYPE, DIREC IN DEPARTAMENTO.DIRECCION%TYPE, 
        NRO IN DEPARTAMENTO.NRO_DPTO%TYPE, CAP IN DEPARTAMENTO.CAPACIDAD%TYPE, COMUNA IN DEPARTAMENTO.ID_COMUNA%TYPE, disp IN DEPARTAMENTO.DISPONIBILIDAD%TYPE, R OUT INTEGER);
        
    PROCEDURE eliminar_dpto(identificador IN DEPARTAMENTO.ID_DPTO%TYPE, R OUT INTEGER);
    
    PROCEDURE listar_dpto(Deptos OUT SYS_REFCURSOR);
END Mantener_Dpto;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Dpto
    AS
    /*Insertar un nuevo departamento*/
    PROCEDURE insertar_dpto(nombre IN DEPARTAMENTO.NOMBRE_DPTO%TYPE, tarifa IN DEPARTAMENTO.TARIFA_DIARIA%TYPE, DIREC IN DEPARTAMENTO.DIRECCION%TYPE, 
        NRO IN DEPARTAMENTO.NRO_DPTO%TYPE, CAP IN DEPARTAMENTO.CAPACIDAD%TYPE, COMUNA IN DEPARTAMENTO.ID_COMUNA%TYPE, disponibilidad IN DEPARTAMENTO.DISPONIBILIDAD%TYPE, R OUT INTEGER)
    IS 
        id_col rowid;
        Dpto_Error_Ag EXCEPTION;
        PRAGMA EXCEPTION_INIT(Dpto_Error_Ag, -20401);
    BEGIN
        INSERT INTO DEPARTAMENTO(NOMBRE_DPTO, TARIFA_DIARIA, DIRECCION, NRO_DPTO, CAPACIDAD, ID_COMUNA, DISPONIBILIDAD) VALUES(nombre, tarifa, direc, nro, cap, COMUNA, disponibilidad) RETURNING rowid INTO id_col;
        /*Retornar un 1 si el insert fue correcto*/
        IF id_col IS NOT NULL THEN
            r:=1;
            COMMIT;
        /*Iniciar un error si no se ingresó*/
        ELSE 
            RAISE Dpto_Error_Ag;
        END IF;
    EXCEPTION 
        WHEN Dpto_Error_Ag THEN
            r:= -20401;
    END;
    
    /*Actualizar un departamento existente*/
    PROCEDURE actualizar_dpto(identificador IN DEPARTAMENTO.ID_DPTO%TYPE, nombre IN DEPARTAMENTO.NOMBRE_DPTO%TYPE, tarifa IN DEPARTAMENTO.TARIFA_DIARIA%TYPE, DIREC IN DEPARTAMENTO.DIRECCION%TYPE, 
        NRO IN DEPARTAMENTO.NRO_DPTO%TYPE, CAP IN DEPARTAMENTO.CAPACIDAD%TYPE, COMUNA IN DEPARTAMENTO.ID_COMUNA%TYPE, disp IN DEPARTAMENTO.DISPONIBILIDAD%TYPE, R OUT INTEGER)
    IS 
        Dpto_Error_Ac EXCEPTION;
        PRAGMA EXCEPTION_INIT(Dpto_Error_Ac, -20402);
    BEGIN
        UPDATE DEPARTAMENTO 
            SET NOMBRE_DPTO = nombre, TARIFA_DIARIA = tarifa, DIRECCION = direc, NRO_DPTO = nro,CAPACIDAD = cap, ID_COMUNA = comuna, DISPONIBILIDAD = disp
        WHERE ID_DPTO =  identificador RETURNING 1 INTO R;
        /*Retornar un 1 si el update fue correcto*/
        IF r = 1 THEN
            COMMIT;
        /*Iniciar un error si no se actualizó*/
        ELSE
            RAISE Dpto_Error_Ac;
        END IF;
    EXCEPTION
        WHEN Dpto_Error_Ac THEN
            r:= -20402;        
    END;
    
    /*Eliminar un departamento existente*/
    PROCEDURE eliminar_dpto(identificador IN DEPARTAMENTO.ID_DPTO%TYPE, R OUT INTEGER)
    IS 
        Dpto_Error_El EXCEPTION;
        PRAGMA EXCEPTION_INIT(Dpto_Error_El, -20403);
    BEGIN 
        DELETE FROM DEPARTAMENTO WHERE ID_DPTO =  identificador RETURNING 1 INTO r;
        /*Retornar un 1 si el delete fue correcto*/
        IF r = 1 THEN
            COMMIT;
        /*Iniciar un error si no se eliminó*/
        ELSE 
            RAISE Dpto_Error_El;
        END IF;
    EXCEPTION
        WHEN Dpto_Error_El THEN
            r:= -20403;
    END;
    
    /*Listar todos los departamentos*/
    PROCEDURE listar_dpto(Deptos OUT SYS_REFCURSOR)
    IS
        v_cant_datos INTEGER;
        Dpto_Error_Li EXCEPTION;
        PRAGMA EXCEPTION_INIT(Dpto_Error_Li, -20404);
    BEGIN
        /*Validar si la tabla tiene datos*/
        SELECT COUNT(*) INTO v_cant_datos FROM DEPARTAMENTO;
        
        /*Si hay datos se consultan*/
        IF v_cant_datos > 0 THEN
            OPEN Deptos FOR
                SELECT * FROM  DEPARTAMENTO DPTO JOIN COMUNA CM ON(cm.id_comuna=dpto.id_comuna);
                
        /*Si la tabla está vacía se inicia un error*/
        ELSE
            RAISE Dpto_Error_Li;
        END IF;
    EXCEPTION
        WHEN Dpto_Error_Li THEN
            Deptos:= NULL;
    END;
END Mantener_Dpto;
/
/*Listar las regiones y comuna*/
CREATE OR REPLACE PACKAGE Ubicacion 
    AS
    PROCEDURE listar_comunas(Comunas out SYS_REFCURSOR);
    PROCEDURE listar_regiones(Regiones out SYS_REFCURSOR);
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
    PROCEDURE listar_regiones(Regiones out SYS_REFCURSOR)
    IS
    BEGIN
        OPEN Regiones FOR
            SELECT * FROM REGION;
    END;
END Ubicacion;
/
/*CRUD Servicios extras*/
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
    /*Agregar un nuevo servicio extra*/
    PROCEDURE insertar_svextra(nombre IN SERVICIO_EXTRA.NOMBRE_SERV_EX%TYPE, descripcion IN SERVICIO_EXTRA.DESC_SERV_EX%TYPE, valor IN SERVICIO_EXTRA.VALOR_SERV_EX%TYPE, R OUT INTEGER)
    IS
        id_col rowid;
        ServE_Error_Ag EXCEPTION;
        PRAGMA EXCEPTION_INIT(ServE_Error_Ag, -20501);
    BEGIN
        INSERT INTO SERVICIO_EXTRA(NOMBRE_SERV_EX, DESC_SERV_EX, VALOR_SERV_EX) VALUES(nombre, descripcion, valor) RETURNING rowid INTO id_col;
        /* Retornar un 1 si el insert fue correcto*/
        IF id_col IS NOT NULL THEN
            r:=1;
            COMMIT;
        /* Iniciar un error si no se ingresó*/
        ELSE
            RAISE ServE_Error_Ag;
        END IF;
    EXCEPTION
        WHEN ServE_Error_Ag THEN
            R:= -20501;
    END;
    
    /*Actualizar un servicio extra existente*/
    PROCEDURE actualizar_svextra(identificador IN SERVICIO_EXTRA.ID_SVC_EX%TYPE, nombre IN SERVICIO_EXTRA.NOMBRE_SERV_EX%TYPE,
        descripcion IN SERVICIO_EXTRA.DESC_SERV_EX%TYPE, valor IN SERVICIO_EXTRA.VALOR_SERV_EX%TYPE, R OUT INTEGER)
    IS
        ServE_Error_Ac EXCEPTION;
        PRAGMA EXCEPTION_INIT(ServE_Error_Ac, -20502);
    BEGIN
        UPDATE SERVICIO_EXTRA 
            SET NOMBRE_SERV_EX = nombre, DESC_SERV_EX = descripcion, VALOR_SERV_EX = valor
        WHERE ID_SVC_EX = identificador RETURNING 1 INTO R;
        /* Retornar un 1 si el update fue correcto*/
        IF r = 1 THEN
            COMMIT;
        /* Iniciar un error si no se actualizó*/
        ELSE 
            RAISE ServE_Error_Ac;
        END IF;
    EXCEPTION
        WHEN ServE_Error_Ac THEN
            R:= -20502;
    END;
    
    /*Eliminar un servicio extra existente*/
    PROCEDURE eliminar_svextra(identificador SERVICIO_EXTRA.ID_SVC_EX%TYPE, R OUT INTEGER)
    IS 
        ServE_Error_El EXCEPTION;
        PRAGMA EXCEPTION_INIT(ServE_Error_El, -20503);
    BEGIN 
        DELETE FROM SERVICIO_EXTRA WHERE ID_SVC_EX =  identificador RETURNING 1 INTO r;
        /*Retornar un 1 si el delete fue correcto*/
        IF r = 1 THEN
            COMMIT;
        /* Iniciar un error si no se eliminó*/
        ELSE
            RAISE ServE_Error_El;
        END IF;
    EXCEPTION
        WHEN ServE_Error_El THEN
            R:= -20503;
    END;
    
    PROCEDURE listar_svextra(Servicios_Ex OUT SYS_REFCURSOR)
    IS
        v_cant_datos INTEGER;
        Dpto_Error_Li EXCEPTION;
        PRAGMA EXCEPTION_INIT(Dpto_Error_Li, -20504);
    BEGIN
        /*Validar si la tabla tiene datos*/
        SELECT COUNT(*) INTO v_cant_datos FROM SERVICIO_EXTRA;
        
        /*Si hay datos se consultan*/
        IF v_cant_datos > 0 THEN
            OPEN Servicios_Ex FOR
                SELECT * FROM SERVICIO_EXTRA;
        ELSE
            RAISE Dpto_Error_Li;
        END IF;
    EXCEPTION
        WHEN Dpto_Error_Li THEN 
            Servicios_Ex:=NULL;
    END;
END Mantener_Servicios_Extras;
/
/*CRUD Inventario*/
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
    /*Insertar objetos a un inventario*/
    PROCEDURE insertar_objeto(id_Dpto IN INVENTARIO_DPTO.ID_DPTO%TYPE, nombre IN INVENTARIO_DPTO.NOMBRE_OBJETO%TYPE,
        cantidad IN INVENTARIO_DPTO.CANT_OBJETO%TYPE, valor IN INVENTARIO_DPTO.VALOR_UNITARIO_OBJ%TYPE, R OUT INTEGER)
    IS
        id_col rowid;
        Inv_Error_Ag EXCEPTION;
        PRAGMA EXCEPTION_INIT(Inv_Error_Ag, -20601);
    BEGIN
        INSERT INTO INVENTARIO_DPTO(ID_DPTO, NOMBRE_OBJETO, CANT_OBJETO, VALOR_UNITARIO_OBJ) VALUES( id_Dpto, nombre, cantidad, valor) RETURNING rowid INTO id_col;
        /* Retornar un 1 si el insert fue correcto*/
        IF id_col IS NOT NULL THEN
            r:=1;
            COMMIT;
        /* Iniciar un error si no se ingresó*/
        ELSE
            RAISE Inv_Error_Ag;
        END IF;
        EXCEPTION
            WHEN Inv_Error_Ag THEN
                R:= -20601;
    END;
    
    /*Actualizar un objeto del inventario*/
    PROCEDURE actualizar_objeto(identificador IN INVENTARIO_DPTO.ID_INV%TYPE, nombre IN INVENTARIO_DPTO.NOMBRE_OBJETO%TYPE,
        cantidad IN INVENTARIO_DPTO.CANT_OBJETO%TYPE, valor IN INVENTARIO_DPTO.VALOR_UNITARIO_OBJ%TYPE, R OUT INTEGER)
    IS
        Inv_Error_Ac EXCEPTION;
        PRAGMA EXCEPTION_INIT(Inv_Error_Ac, -20602);        
    BEGIN
        UPDATE INVENTARIO_DPTO 
            SET NOMBRE_OBJETO = nombre, CANT_OBJETO = cantidad, VALOR_UNITARIO_OBJ = valor
        WHERE ID_INV = identificador RETURNING 1 INTO R;
        /* Retornar un 1 si el update fue correcto*/
        IF r = 1 THEN
            COMMIT;
        ELSE
            RAISE Inv_Error_Ac;
        END IF;
        EXCEPTION
            WHEN Inv_Error_Ac THEN
                R:= -20602;
    END;
    
    /*Eliminar un objeto del inventario*/
    PROCEDURE eliminar_objeto(identificador IN INVENTARIO_DPTO.ID_INV%TYPE, R OUT INTEGER)
    IS 
        Inv_Error_El EXCEPTION;
        PRAGMA EXCEPTION_INIT(Inv_Error_El, -20603);        
    BEGIN 
        DELETE FROM INVENTARIO_DPTO WHERE ID_INV = identificador RETURNING 1 INTO r;
        /* Retornar un 1 si el delete fue correcto*/
        IF r = 1 THEN
            COMMIT;
        /* Iniciar un error si no se eliminó*/
        ELSE
            RAISE Inv_Error_El;
        END IF;
        EXCEPTION
            WHEN Inv_Error_El THEN
                R:= -20603;    
    END;
    /*Listar todos los objetos del inventario*/
    PROCEDURE listar_inventario(id_dp IN INVENTARIO_DPTO.ID_DPTO%TYPE, Inventario OUT SYS_REFCURSOR)
        IS
        v_cant_datos INTEGER;
        Inv_Error_Li EXCEPTION;
        PRAGMA EXCEPTION_INIT(Inv_Error_Li, -20604);       
    BEGIN
        /*Validar si la tabla tiene datos*/
        SELECT COUNT(*) INTO v_cant_datos FROM INVENTARIO_DPTO;
        
        /*Si hay datos se consultan*/
        IF v_cant_datos > 0 THEN
            OPEN Inventario FOR
                SELECT * FROM INVENTARIO_DPTO WHERE ID_DPTO =id_dp;
        /* Si la tabla está vacía se inicia un error*/
        ELSE
            RAISE Inv_Error_Li;
        END IF;
        EXCEPTION
            WHEN Inv_Error_Li THEN
                Inventario:= null;    
    END;
END Mantener_Inventario_Dpto;
/

/*CRUD Imagenes departamento*/
CREATE OR REPLACE PACKAGE Mantener_Img AS
    PROCEDURE Agregar_Img(id_dp IN FOTOGRAFIA_DPTO.ID_DPTO%TYPE, path_img IN FOTOGRAFIA_DPTO.FOTO_PATH%TYPE, alt_img IN FOTOGRAFIA_DPTO.ALT_FOTO%TYPE, extension IN VARCHAR2, NEW_FILE OUT VARCHAR2, R OUT INTEGER);
    PROCEDURE Listar_Img(id_dp IN FOTOGRAFIA_DPTO.ID_DPTO%TYPE, Imagenes OUT SYS_REFCURSOR);
END Mantener_Img;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Img AS
    /*Agregar una imagen a un departamento*/
    PROCEDURE Agregar_Img(id_dp IN FOTOGRAFIA_DPTO.ID_DPTO%TYPE, path_img IN FOTOGRAFIA_DPTO.FOTO_PATH%TYPE, alt_img IN FOTOGRAFIA_DPTO.ALT_FOTO%TYPE, extension IN VARCHAR2, NEW_FILE OUT VARCHAR2, R OUT INTEGER)
    IS
        Imagen_Error_In EXCEPTION;
        PRAGMA EXCEPTION_INIT(Imagen_Error_In, -20701);    
    BEGIN 
        /*Iniciar un punto de guardado, en caso de un error se vuelve a este punto*/
        SAVEPOINT A;
        /*Agregar la imagen sin ruta*/
        INSERT INTO FOTOGRAFIA_DPTO(ID_DPTO, ALT_FOTO) VALUES(id_dp, alt_img) RETURNING ID_FOTO INTO R;
        /*Agregar la ruta de la imagen*/
        UPDATE FOTOGRAFIA_DPTO SET FOTO_PATH = path_img||r||extension WHERE ID_FOTO = R RETURNING 1, r||extension INTO r, NEW_FILE;
        /*Si ambos pasos ocurren con éxito se confirman los cambios*/    
        IF r = 1 THEN
            COMMIT;
        /* Iniciar un error si no se ingresó*/
        ELSE
            RAISE Imagen_Error_In;
        END IF;
    EXCEPTION
        /*Regresar al punto A*/
        WHEN Imagen_Error_In THEN
            R:= -20701;
            ROLLBACK TO A;
    END;
    
    PROCEDURE Listar_Img(id_dp IN FOTOGRAFIA_DPTO.ID_DPTO%TYPE, Imagenes OUT SYS_REFCURSOR)
    IS
        v_cant_datos INTEGER;
        Imagen_Error_Li EXCEPTION;
        PRAGMA EXCEPTION_INIT(Imagen_Error_Li, -20704);       
    BEGIN
        /*Validar si la tabla tiene datos*/
        SELECT COUNT(*) INTO v_cant_datos FROM FOTOGRAFIA_DPTO;
        
        /*Si hay datos se consultan*/
        IF v_cant_datos > 0 THEN
            OPEN Imagenes FOR
                SELECT * FROM FOTOGRAFIA_DPTO WHERE ID_DPTO =id_dp;
        /* Si la tabla está vacía se inicia un error*/
        ELSE
            RAISE Imagen_Error_Li;
        END IF;
    EXCEPTION
            WHEN Imagen_Error_Li THEN
                Imagenes:= null;    
    END;
END Mantener_Img;
/
/*CRUD Administradores*/
CREATE OR REPLACE PACKAGE Mantener_Usuario_Admin AS
    PROCEDURE Agregar_Admin(email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEÑA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        rut IN ADMINISTRADOR.RUT_ADMIN%TYPE, nombre IN ADMINISTRADOR.NOMBRES_ADMIN%TYPE, apellido IN  ADMINISTRADOR.APELLIDOS_ADMIN%TYPE, R OUT INT);
    PROCEDURE Actualizar_Admin(id_usr IN USUARIO.ID_USUARIO%TYPE, email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEÑA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        nombre IN ADMINISTRADOR.NOMBRES_ADMIN%TYPE, apellido IN  ADMINISTRADOR.APELLIDOS_ADMIN%TYPE, R OUT INT);
    PROCEDURE Eliminar_Admin(id_usr IN USUARIO.ID_USUARIO%TYPE, R OUT INT);
    PROCEDURE Listar_Admin(Administradores OUT SYS_REFCURSOR);
END Mantener_Usuario_Admin;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Usuario_Admin AS
    /*Agregar un usuario tipo administrador*/
    PROCEDURE Agregar_Admin(email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEÑA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        rut IN ADMINISTRADOR.RUT_ADMIN%TYPE, nombre IN ADMINISTRADOR.NOMBRES_ADMIN%TYPE, apellido IN  ADMINISTRADOR.APELLIDOS_ADMIN%TYPE, R OUT INT)
    IS
        id_col rowid;
        identificador_usr USUARIO.ID_USUARIO%TYPE;
        identificador_admin ADMINISTRADOR.ID_ADMIN%TYPE;
        v_pass VARCHAR2(40);
        error_crear_usuario EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_crear_usuario, -20001);
        error_crear_admin EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_crear_admin, -20201);
    BEGIN 
        /*Iniciar un punto de guardado, en caso de un error se vuelve a este punto*/
        SAVEPOINT A;
        
        /*Generar contraseña */
        v_pass:=GENERAR_CON(email_c, pass);
        INSERT INTO USUARIO(EMAIL, CONTRASEÑA, TELEFONO) VALUES(email_c, v_pass , fono) RETURNING rowid, ID_USUARIO INTO id_col, identificador_usr;
        
        /*Si el usuario fue creado, se inicia la creación de su rol*/
        IF id_col IS NOT NULL THEN
            INSERT INTO ADMINISTRADOR(RUT_ADMIN,NOMBRES_ADMIN,APELLIDOS_ADMIN,ID_USUARIO) VALUES(rut, nombre, apellido, identificador_usr) 
                RETURNING rowid,ID_ADMIN INTO id_col,identificador_admin;
            /*Una vez creado su rol, se empareja con el usuario*/
            IF id_col IS NOT NULL THEN
                UPDATE USUARIO SET ID_ADMIN = identificador_admin WHERE ID_USUARIO = identificador_usr RETURNING 1 INTO r;
                IF r = 1 THEN
                    COMMIT;        
                END IF;
            /*De no haber sido creado el rol se inicia un error*/
            ELSE 
                RAISE error_crear_admin;
            END IF;
        /*De no haber sido creado el usuario se inicar un error*/
        ELSE
            RAISE error_crear_usuario;
        END IF;
    EXCEPTION
        /*Se retorna -1 si el valor está repetido y se vuelve al punto A*/
        WHEN DUP_VAL_ON_INDEX THEN
            ROLLBACK TO A;
            R:= -1;
        /*Se vuelve al punto A y se desechan los cambios*/
        WHEN error_crear_usuario THEN 
            ROLLBACK TO A;
            R:= -20001;
        WHEN error_crear_admin THEN
            ROLLBACK TO A;
            R:= -20201;
    END;
    /*Actualizar un usuario administrador*/
    PROCEDURE Actualizar_Admin(id_usr IN USUARIO.ID_USUARIO%TYPE, email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEÑA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        nombre IN ADMINISTRADOR.NOMBRES_ADMIN%TYPE, apellido IN  ADMINISTRADOR.APELLIDOS_ADMIN%TYPE, R OUT INT)
    IS
        v_pass VARCHAR2(40);
        error_actualizar_usuario EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_actualizar_usuario, -20002);
        error_actualizar_admin EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_actualizar_admin, -20202);
    BEGIN
        /*Iniciar un punto de guardado, en caso de un error se vuelve a este punto*/
        SAVEPOINT A;
        /*Generar contraseña */
        v_pass:=GENERAR_CON(email_c, pass);
        UPDATE USUARIO SET EMAIL = email_c, CONTRASEÑA = v_pass, TELEFONO = fono WHERE ID_USUARIO = id_usr RETURNING 1 INTO R;
        
        /*Si el usuario fue actualizado, se inicia la actualizacion de su rol*/
        IF R = 1 THEN
            UPDATE ADMINISTRADOR SET NOMBRES_ADMIN = nombre, APELLIDOS_ADMIN = apellido WHERE ID_USUARIO = id_usr RETURNING 1 INTO R;
            /*Si ambas actualizaciones fueron realizadas con éxito, se confirman los cambios*/
            IF R = 1 THEN
                COMMIT;
            /*Si la actualización del rol falla se inicia un error*/
            ELSE
                RAISE error_actualizar_admin;
            END IF;
        /*Si la actualización del usuario falla se inicia un error*/
        ELSE
            RAISE error_actualizar_usuario;
        END IF;
    EXCEPTION
        /*Se retorna -1 si el valor está repetido y se vuelve al punto A*/
        WHEN DUP_VAL_ON_INDEX THEN
            ROLLBACK TO A;
            R:= -1;
        /*Se vuelve al punto A y se desechan los cambios*/
        WHEN error_actualizar_usuario THEN 
            ROLLBACK TO A;
            R:= -20002;
        WHEN error_actualizar_admin THEN
            ROLLBACK TO A;
            R:= -20202;
    END;
    /*Eliminar un usuario administrador*/
    PROCEDURE Eliminar_Admin(id_usr IN USUARIO.ID_USUARIO%TYPE, R OUT INT)
    IS
        error_eliminar_usuario EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_eliminar_usuario, -20003);
    BEGIN
        /*Iniciar un punto de guardado, en caso de un error se vuelve a este punto*/
        SAVEPOINT A;
        DELETE FROM USUARIO WHERE ID_USUARIO = id_usr RETURNING 1 INTO R;
        /* Iniciar un error si no se eliminó*/
        IF R = 1 THEN
            COMMIT;      
        /* Iniciar un error si no se eliminó*/
        ELSE
            RAISE error_eliminar_usuario;
        END IF;
    EXCEPTION
        WHEN error_eliminar_usuario THEN
            ROLLBACK TO A;
            R:= -20003;
    END;
    /*Listar todos los administradores*/
    PROCEDURE Listar_Admin(Administradores OUT SYS_REFCURSOR)
    IS
        v_cant_datos INTEGER;
        error_listar_admin EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_listar_admin, -20204); 
    BEGIN
        /* Validar si la tabla tiene datos*/
        SELECT COUNT(*) INTO v_cant_datos FROM ADMINISTRADOR;
        /* Si hay datos se consultan*/
        IF v_cant_datos>0 THEN
            OPEN Administradores FOR
                SELECT * FROM USUARIO USR JOIN ADMINISTRADOR ADM ON(USR.ID_USUARIO = ADM.ID_USUARIO);
        /* Si la tabla está vacía se inicia un error*/
        ELSE
            RAISE error_listar_admin;
        END IF;
    EXCEPTION
        WHEN error_listar_admin THEN
            Administradores:= null;
    END;
            
END Mantener_Usuario_Admin;
/
CREATE OR REPLACE PACKAGE Mantener_Usuario_Cliente AS
    PROCEDURE Agregar_Cliente(email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEÑA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        rut IN CLIENTE.RUT_CLIENTE%TYPE, nombre IN CLIENTE.NOMBRES_CLIENTE%TYPE, apellido IN  CLIENTE.APELLIDOS_CLIENTE%TYPE, R OUT INT);
    PROCEDURE Actualizar_Cliente(id_usr IN USUARIO.ID_USUARIO%TYPE, email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEÑA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        nombre IN CLIENTE.NOMBRES_CLIENTE%TYPE, apellido IN  CLIENTE.APELLIDOS_CLIENTE%TYPE, R OUT INT);
    PROCEDURE Eliminar_Cliente(id_usr IN USUARIO.ID_USUARIO%TYPE, R OUT INT);
    PROCEDURE Listar_Cliente(Clientes OUT SYS_REFCURSOR);
END Mantener_Usuario_Cliente;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Usuario_Cliente AS
    /*Agregar un usuario tipo cliente*/
    PROCEDURE Agregar_Cliente(email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEÑA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        rut IN CLIENTE.RUT_CLIENTE%TYPE, nombre IN CLIENTE.NOMBRES_CLIENTE%TYPE, apellido IN  CLIENTE.APELLIDOS_CLIENTE%TYPE, R OUT INT)
    IS
        id_col rowid;
        identificador_usr USUARIO.ID_USUARIO%TYPE;
        identificador_cliente CLIENTE.ID_CLIENTE%TYPE;
        v_pass VARCHAR2(40);
        error_crear_usuario EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_crear_usuario, -20001);
        error_crear_cliente EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_crear_cliente, -20101);
    BEGIN 
        /*Iniciar un punto de guardado, en caso de un error se vuelve a este punto*/
        SAVEPOINT A;
        
        /*Generar contraseña */
        v_pass:=GENERAR_CON(email_c, pass);
        INSERT INTO USUARIO(EMAIL, CONTRASEÑA, TELEFONO) VALUES(email_c, v_pass , fono) RETURNING rowid, ID_USUARIO INTO id_col, identificador_usr;
        
        /*Si el usuario fue creado, se inicia la creación de su rol*/ 
        IF id_col IS NOT NULL THEN
            /*Una vez creado su rol, se empareja con el usuario*/
            INSERT INTO CLIENTE(RUT_CLIENTE,NOMBRES_CLIENTE,APELLIDOS_CLIENTE,ID_USUARIO) VALUES(rut, nombre, apellido, identificador_usr) 
                RETURNING rowid,ID_CLIENTE INTO id_col,identificador_cliente;
            IF id_col IS NOT NULL THEN
                UPDATE USUARIO SET ID_CLIENTE = identificador_cliente WHERE ID_USUARIO = identificador_usr RETURNING 1 INTO r;
                IF r = 1 THEN
                    COMMIT;        
                END IF;
            /*De no haber sido creado el rol se inicia un error*/
            ELSE 
                RAISE error_crear_cliente;
            END IF;
        /*De no haber sido creado el usuario se inicar un error*/
        ELSE
            RAISE error_crear_usuario;
        END IF;
    EXCEPTION
        /*Se retorna -1 si el valor está repetido y se vuelve al punto A*/
        WHEN DUP_VAL_ON_INDEX THEN
            ROLLBACK TO A;
            R:= -1;
         /*Se vuelve al punto A y se desechan los cambios*/
        WHEN error_crear_usuario THEN 
            ROLLBACK TO A;
            R:= -20001;
        WHEN error_crear_cliente THEN
            ROLLBACK TO A;
            R:= -20101;
    END;
    /*Actualizar un usuario tipo cliente*/
    PROCEDURE Actualizar_Cliente(id_usr IN USUARIO.ID_USUARIO%TYPE, email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEÑA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        nombre IN CLIENTE.NOMBRES_CLIENTE%TYPE, apellido IN  CLIENTE.APELLIDOS_CLIENTE%TYPE, R OUT INT)
    IS
        v_pass VARCHAR2(40);
        error_actualizar_usuario EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_actualizar_usuario, -20002);
        error_actualizar_funcionario EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_actualizar_cliente, -20302);
    BEGIN
        /*Iniciar un punto de guardado, en caso de un error se vuelve a este punto*/
        SAVEPOINT A;
        
        /*Generar contraseña */
        v_pass:=GENERAR_CON(email_c, pass);
        /*Si el usuario fue actualizado, se inicia la actualización de su rol*/
        UPDATE USUARIO SET EMAIL = email_c, CONTRASEÑA = v_pass, TELEFONO = fono WHERE ID_USUARIO = id_usr RETURNING 1 INTO R;
        IF R = 1 THEN
            UPDATE CLIENTE SET NOMBRES_CLIENTE = nombre, APELLIDOS_CLIENTE = apellido WHERE ID_USUARIO = id_usr RETURNING 1 INTO R;
            /*Si ambas actualizaciones fueron realizadas con éxito, se confirman los cambios*/
            IF R = 1 THEN
                COMMIT;
            /*Si la actualización del rol falla se inicia un error*/
            ELSE
                RAISE error_actualizar_cliente;
            END IF;
        /*Si la actualización del usuario falla se inicia un error*/
        ELSE 
            RAISE error_actualizar_usuario;
        END IF;
    EXCEPTION 
        /*Se retorna -1 si el valor está repetido y se vuelve al punto A*/
        WHEN DUP_VAL_ON_INDEX THEN
            ROLLBACK TO A;
            R:= -1;
        /*Se vuelve al punto A y se desechan los cambios*/
        WHEN error_actualizar_usuario THEN 
            ROLLBACK TO A;
            R:= -20002;
        WHEN error_actualizar_cliente THEN
            ROLLBACK TO A;
            R:= -20202;       
    END;
    
    /*Eliminar un usuario cliente existente*/
    PROCEDURE Eliminar_Cliente(id_usr IN USUARIO.ID_USUARIO%TYPE, R OUT INT)
    IS
        error_eliminar_cliente EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_eliminar_cliente, -20103);
    BEGIN
        SAVEPOINT A;
        DELETE FROM USUARIO WHERE ID_USUARIO = id_usr RETURNING 1 INTO R;
        IF R = 1 THEN
            COMMIT;       
        ELSE 
            RAISE error_eliminar_cliente;
        END IF;
    EXCEPTION
        WHEN error_eliminar_cliente THEN
            R:= -20103;
    END;
    
    /*Listar los clientes*/
    PROCEDURE Listar_Cliente(Clientes OUT SYS_REFCURSOR)
    IS
        v_cant_datos INTEGER;
        error_listar_clientes EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_listar_clientes, -20104); 
    BEGIN
        /* Validar si la tabla tiene datos*/
        SELECT COUNT(*) INTO v_cant_datos FROM CLIENTE;
        /* Si hay datos se consultan*/
        IF v_cant_datos>0 THEN
            OPEN Clientes FOR
                SELECT * FROM USUARIO USR JOIN CLIENTE CLI ON(USR.ID_USUARIO = CLI.ID_USUARIO);
        /* Si la tabla está vacía se inicia un error*/
        ELSE
            RAISE error_listar_clientes;
        END IF;
    EXCEPTION
        WHEN error_listar_clientes THEN
            Clientes:= null;
    END;
            
END Mantener_Usuario_Cliente;
/
CREATE OR REPLACE PACKAGE Mantener_Usuario_Funcionario AS
    PROCEDURE Agregar_Funcionario(email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEÑA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        rut IN FUNCIONARIO.RUT_FUNCIONARIO%TYPE, nombre IN FUNCIONARIO.NOMBRES_FUNCIONARIO%TYPE, apellido IN  FUNCIONARIO.APELLIDOS_FUNCIONARIO%TYPE, R OUT INT);
    PROCEDURE Actualizar_Funcionario(id_usr IN USUARIO.ID_USUARIO%TYPE, email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEÑA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        nombre IN FUNCIONARIO.NOMBRES_FUNCIONARIO%TYPE, apellido IN  FUNCIONARIO.APELLIDOS_FUNCIONARIO%TYPE, R OUT INT);
    PROCEDURE Eliminar_Funcionario(id_usr IN USUARIO.ID_USUARIO%TYPE, R OUT INT);
    PROCEDURE Listar_Funcionario(Funcionarios OUT SYS_REFCURSOR);
END Mantener_Usuario_Funcionario;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Usuario_Funcionario AS
    /*Agregar un usuario tipo funcionario*/
    PROCEDURE Agregar_Funcionario(email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEÑA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        rut IN FUNCIONARIO.RUT_FUNCIONARIO%TYPE, nombre IN FUNCIONARIO.NOMBRES_FUNCIONARIO%TYPE, apellido IN  FUNCIONARIO.APELLIDOS_FUNCIONARIO%TYPE, R OUT INT)
    IS
        id_col rowid;
        identificador_usr USUARIO.ID_USUARIO%TYPE;
        identificador_funcionario FUNCIONARIO.ID_FUNCIONARIO%TYPE;
        v_pass VARCHAR2(40);
        error_crear_usuario EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_crear_usuario, -20001);
        error_crear_funcionario EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_crear_funcionario, -20301);
    BEGIN 
        /*Iniciar un punto de guardado, en caso de un error se vuelve a este punto*/
        SAVEPOINT A;
        
        /*Generar contraseña */
        v_pass:=GENERAR_CON(email_c, pass);
        INSERT INTO USUARIO(EMAIL, CONTRASEÑA, TELEFONO) VALUES(email_c, v_pass , fono) RETURNING rowid, ID_USUARIO INTO id_col, identificador_usr;
        IF id_col IS NOT NULL THEN
            /*Una vez creado su rol, se empareja con el usuario*/
            INSERT INTO FUNCIONARIO(RUT_FUNCIONARIO,NOMBRES_FUNCIONARIO,APELLIDOS_FUNCIONARIO,ID_USUARIO) VALUES(rut, nombre, apellido, identificador_usr) 
                RETURNING rowid,ID_FUNCIONARIO INTO id_col,identificador_funcionario;
            IF id_col IS NOT NULL THEN
                UPDATE USUARIO SET ID_FUNCIONARIO = identificador_funcionario WHERE ID_USUARIO = identificador_usr RETURNING 1 INTO r;
                IF r = 1 THEN
                    COMMIT;        
                END IF;
            /*De no haber sido creado el rol se inicia un error*/
            ELSE 
                RAISE error_crear_funcionario;
            END IF;
        /*De no haber sido creado el usuario se inicar un error*/
        ELSE
            RAISE error_crear_usuario;
        END IF;
    EXCEPTION
        /*Se retorna -1 si el valor está repetido y se vuelve al punto A*/
        WHEN DUP_VAL_ON_INDEX THEN
            ROLLBACK TO A;
            R:= -1;
         /*Se vuelve al punto A y se desechan los cambios*/
        WHEN error_crear_usuario THEN 
            ROLLBACK TO A;
            R:= -20001;
        WHEN error_crear_funcionario THEN
            ROLLBACK TO A;
            R:= -20301;
    END;
    PROCEDURE Actualizar_Funcionario(id_usr IN USUARIO.ID_USUARIO%TYPE, email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEÑA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        nombre IN FUNCIONARIO.NOMBRES_FUNCIONARIO%TYPE, apellido IN  FUNCIONARIO.APELLIDOS_FUNCIONARIO%TYPE, R OUT INT)
    IS
        v_pass VARCHAR2(40);
        error_actualizar_usuario EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_actualizar_usuario, -20002);
        error_actualizar_funcionario EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_actualizar_funcionario, -20302);
    BEGIN
        /*Iniciar un punto de guardado, en caso de un error se vuelve a este punto*/
        SAVEPOINT A;
        /*Generar contraseña */
        v_pass:=GENERAR_CON(email_c, pass);
        UPDATE USUARIO SET EMAIL = email_c, CONTRASEÑA = v_pass, TELEFONO = fono WHERE ID_USUARIO = id_usr RETURNING 1 INTO R;
        
        /*Si el usuario fue actualizado, se inicia la actualizacion de su rol*/
        IF R = 1 THEN
            UPDATE FUNCIONARIO SET NOMBRES_FUNCIONARIO = nombre, APELLIDOS_FUNCIONARIO = apellido WHERE ID_USUARIO = id_usr RETURNING 1 INTO R;
            /*Si ambas actualizaciones fueron realizadas con éxito, se confirman los cambios*/
            IF R = 1 THEN
                COMMIT;
            /*Si la actualización del rol falla se inicia un error*/
            ELSE
                RAISE error_actualizar_funcionario;
            END IF;
        /*Si la actualización del usuario falla se inicia un error*/
        ELSE 
            RAISE error_actualizar_usuario;
        END IF;
    EXCEPTION 
        /*Se retorna -1 si el valor está repetido y se vuelve al punto A*/
        WHEN DUP_VAL_ON_INDEX THEN
            ROLLBACK TO A;
            R:= -1;
        /*Se vuelve al punto A y se desechan los cambios*/
        WHEN error_actualizar_usuario THEN 
            ROLLBACK TO A;
            R:= -20002;
        WHEN error_actualizar_funcionario THEN
            ROLLBACK TO A;
            R:= -20202;       
    END;
    
    /*Eliminar un funcionario*/
    PROCEDURE Eliminar_Funcionario(id_usr IN USUARIO.ID_USUARIO%TYPE, R OUT INT)
    IS
        error_eliminar_funcionario EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_eliminar_funcionario, -20303);
    BEGIN
        SAVEPOINT A;
        DELETE FROM USUARIO WHERE ID_USUARIO = id_usr RETURNING 1 INTO R;
        IF R = 1 THEN
            COMMIT;       
        ELSE 
            RAISE error_eliminar_funcionario;
        END IF;
    EXCEPTION
        WHEN error_eliminar_funcionario THEN
            R:= -20303;
    END;
    
    /*Listar los funcionario*/
    PROCEDURE Listar_Funcionario(Funcionarios OUT SYS_REFCURSOR)
    IS
        v_cant_datos INTEGER;
        error_listar_funcionarios EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_listar_funcionarios, -20304); 
    BEGIN
        /* Validar si la tabla tiene datos*/
        SELECT COUNT(*) INTO v_cant_datos FROM FUNCIONARIO;
        /* Si hay datos se consultan*/
        IF v_cant_datos>0 THEN
            OPEN Funcionarios FOR
                SELECT * FROM USUARIO USR JOIN FUNCIONARIO FUN ON(USR.ID_USUARIO = FUN.ID_USUARIO);
        /* Si la tabla está vacía se inicia un error*/
        ELSE
            RAISE error_listar_funcionarios;
        END IF;
    EXCEPTION
        WHEN error_listar_funcionarios THEN
            Funcionarios:= null;
    END;
            
END Mantener_Usuario_Funcionario;
/
CREATE OR REPLACE PACKAGE Mantener_Mantenimiento
AS
    PROCEDURE Agregar_Mantenimiento(id_depto MANTENIMIENTO.ID_DPTO%TYPE, nombre MANTENIMIENTO.NOMBRE_MANT%TYPE, descripcion MANTENIMIENTO.DESC_MANT%TYPE,
        fecha_ini MANTENIMIENTO.FECHA_INICIO%TYPE, fecha_fin MANTENIMIENTO.FECHAR_TERMINO%TYPE, estado_man MANTENIMIENTO.ESTADO%TYPE, costo MANTENIMIENTO.COSTO_MANTENCION%TYPE, R OUT INTEGER);
    PROCEDURE Actualizar_Mantenimiento(id_mantenimiento MANTENIMIENTO.ID_MANT%TYPE, nombre MANTENIMIENTO.NOMBRE_MANT%TYPE, descripcion MANTENIMIENTO.DESC_MANT%TYPE,
        fecha_ini MANTENIMIENTO.FECHA_INICIO%TYPE, fecha_fin MANTENIMIENTO.FECHAR_TERMINO%TYPE, estado_man MANTENIMIENTO.ESTADO%TYPE, costo MANTENIMIENTO.COSTO_MANTENCION%TYPE, R OUT INTEGER);
    PROCEDURE Eliminar_Mantenimiento(id_mantenimiento MANTENIMIENTO.ID_MANT%TYPE,R OUT INTEGER);
    PROCEDURE Listar_Mantenimientos(id_depto MANTENIMIENTO.ID_DPTO%TYPE,Mantenimientos OUT SYS_REFCURSOR);    
END Mantener_Mantenimiento;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Mantenimiento
AS 
    /*Insertar un nuevo mantenimiento a un depto*/
    PROCEDURE Agregar_Mantenimiento(id_depto IN MANTENIMIENTO.ID_DPTO%TYPE, nombre IN MANTENIMIENTO.NOMBRE_MANT%TYPE, descripcion IN MANTENIMIENTO.DESC_MANT%TYPE,
        fecha_ini IN MANTENIMIENTO.FECHA_INICIO%TYPE, fecha_fin IN MANTENIMIENTO.FECHAR_TERMINO%TYPE, estado_man IN MANTENIMIENTO.ESTADO%TYPE, costo IN MANTENIMIENTO.COSTO_MANTENCION%TYPE, R OUT INTEGER)
    IS
        id_col rowid;
        Mantenimiento_Error_In EXCEPTION;
        PRAGMA EXCEPTION_INIT(Mantenimiento_Error_In, -20801);    
    BEGIN
        INSERT INTO MANTENIMIENTO(ID_DPTO, NOMBRE_MANT, DESC_MANT, FECHA_INICIO, FECHAR_TERMINO, ESTADO, COSTO_MANTENCION) 
            VALUES(id_depto, nombre, descripcion, fecha_ini, fecha_fin, estado_man, costo) RETURNING rowid INTO id_col;
        /* Retornar un 1 si el insert fue correcto*/
        IF id_col IS NOT NULL THEN
            R:=1;
            COMMIT;
        /* Iniciar un error si no se ingresó*/
        ELSE
            RAISE Mantenimiento_Error_In;
        END IF;
    EXCEPTION
        WHEN Mantenimiento_Error_In THEN
            R:=-20801;
    END;
    
    /*Actualizar un mantenimiento existente*/
    PROCEDURE Actualizar_Mantenimiento(id_mantenimiento MANTENIMIENTO.ID_MANT%TYPE, nombre MANTENIMIENTO.NOMBRE_MANT%TYPE, descripcion MANTENIMIENTO.DESC_MANT%TYPE,
        fecha_ini MANTENIMIENTO.FECHA_INICIO%TYPE, fecha_fin MANTENIMIENTO.FECHAR_TERMINO%TYPE, estado_man MANTENIMIENTO.ESTADO%TYPE, costo MANTENIMIENTO.COSTO_MANTENCION%TYPE, R OUT INTEGER)
    IS
        Mantenimiento_Error_Ac EXCEPTION;
        PRAGMA EXCEPTION_INIT(Mantenimiento_Error_Ac, -20802);  
    BEGIN
        UPDATE MANTENIMIENTO SET NOMBRE_MANT = nombre, DESC_MANT = descripcion, FECHA_INICIO = fecha_ini, FECHAR_TERMINO = fecha_fin, ESTADO = estado_man,
            COSTO_MANTENCION = costo WHERE ID_MANT = id_mantenimiento RETURNING 1 INTO R;
        /* Retornar un 1 si el update fue correcto*/
        IF r = 1 THEN
            COMMIT;      
        /* Iniciar un error si no se actualizó*/    
        ELSE
            RAISE Mantenimiento_Error_Ac;
        END IF;
    EXCEPTION 
        WHEN Mantenimiento_Error_Ac THEN 
            R:=-20802;
    END;
    
    /*Eliminar un mantenimiento existente*/
    PROCEDURE Eliminar_Mantenimiento(id_mantenimiento MANTENIMIENTO.ID_MANT%TYPE,R OUT INTEGER)
    IS
        Mantenimiento_Error_El EXCEPTION;
        PRAGMA EXCEPTION_INIT(Mantenimiento_Error_El, -20803);  
    BEGIN
        DELETE FROM MANTENIMIENTO WHERE ID_MANT = id_mantenimiento RETURNING 1 INTO R;
        /* Retornar un 1 si el delete fue correcto*/
        IF R = 1 THEN
            COMMIT;
        /* Iniciar un error si no se eliminó*/
        ELSE
            RAISE Mantenimiento_Error_El;
        END IF;
    EXCEPTION
        WHEN Mantenimiento_Error_El THEN    
            R:=-20803;
    END;
    
    /*Listar todos los mantenimientos*/
    PROCEDURE Listar_Mantenimientos(id_depto MANTENIMIENTO.ID_DPTO%TYPE, Mantenimientos OUT SYS_REFCURSOR)
    IS
        v_cant_datos INTEGER;
        Mantenimiento_Error_Li EXCEPTION;
        PRAGMA EXCEPTION_INIT(Mantenimiento_Error_Li, -20804); 
    BEGIN
        /* Validar si la tabla tiene datos*/
        SELECT COUNT(*) INTO v_cant_datos FROM FUNCIONARIO;
        /* Si hay datos se consultan*/
        IF v_cant_datos>0 THEN
            OPEN Mantenimientos FOR
                SELECT id_mant, nombre_mant,desc_mant,to_char(fecha_inicio, 'dd/MM/yyyy'), to_char(fechar_termino, 'dd/MM/yyyy'), costo_mantencion, estado FROM MANTENIMIENTO WHERE ID_DPTO = id_depto;
        /* Si la tabla está vacía se inicia un error*/
        ELSE
            RAISE Mantenimiento_Error_Li;
        END IF;
    EXCEPTION
        WHEN Mantenimiento_Error_Li THEN
            Mantenimientos:= null;
    END;
    
END Mantener_Mantenimiento;
/
CREATE OR REPLACE PACKAGE Mantener_Tours
    AS
    PROCEDURE insertar_tour(nombre IN TOUR_PLAN.NOMBRE_TOUR%TYPE, descripcion IN TOUR_PLAN.DESC_TOUR%TYPE, valor IN TOUR_PLAN.VALOR_TOUR%TYPE, region IN TOUR_PLAN.ID_REGION%TYPE, R OUT INTEGER);
    PROCEDURE actualizar_tour(identificador IN TOUR_PLAN.ID_TOUR%TYPE, nombre IN TOUR_PLAN.NOMBRE_TOUR%TYPE, descripcion IN TOUR_PLAN.DESC_TOUR%TYPE, 
    valor IN TOUR_PLAN.VALOR_TOUR%TYPE, region IN TOUR_PLAN.ID_REGION%TYPE, R OUT INTEGER);
    PROCEDURE eliminar_tour(identificador TOUR_PLAN.ID_TOUR%TYPE, R OUT INTEGER);
    PROCEDURE listar_tour(Tours OUT SYS_REFCURSOR);

END Mantener_Tours;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Tours
    AS
    PROCEDURE insertar_tour(nombre IN TOUR_PLAN.NOMBRE_TOUR%TYPE, descripcion IN TOUR_PLAN.DESC_TOUR%TYPE, valor IN TOUR_PLAN.VALOR_TOUR%TYPE, region IN TOUR_PLAN.ID_REGION%TYPE, R OUT INTEGER)
    IS
        id_col rowid;
    BEGIN
        INSERT INTO TOUR_PLAN(NOMBRE_TOUR, DESC_TOUR, VALOR_TOUR, ID_REGION) VALUES(nombre, descripcion, valor, region) RETURNING rowid INTO id_col;
        IF id_col IS NOT NULL THEN
            r:=1;
            COMMIT;
        END IF;
    END;
    PROCEDURE actualizar_tour(identificador IN TOUR_PLAN.ID_TOUR%TYPE, nombre IN TOUR_PLAN.NOMBRE_TOUR%TYPE, descripcion IN TOUR_PLAN.DESC_TOUR%TYPE, 
    valor IN TOUR_PLAN.VALOR_TOUR%TYPE, region IN TOUR_PLAN.ID_REGION%TYPE, R OUT INTEGER)
    IS
    BEGIN
        UPDATE TOUR_PLAN 
            SET NOMBRE_TOUR = nombre, DESC_TOUR = descripcion, VALOR_TOUR = valor, ID_REGION = region
        WHERE ID_TOUR = identificador RETURNING 1 INTO R;
        IF r = 1 THEN
            COMMIT;
        END IF;
    END;
    PROCEDURE eliminar_tour(identificador TOUR_PLAN.ID_TOUR%TYPE, R OUT INTEGER)
    IS 
    BEGIN 
        DELETE FROM TOUR_PLAN WHERE ID_TOUR =  identificador RETURNING 1 INTO r;
        IF r = 1 THEN
            COMMIT;
        END IF;
    END;
    PROCEDURE listar_tour(Tours OUT SYS_REFCURSOR)
    IS
    BEGIN
        OPEN Tours FOR
            SELECT * FROM TOUR_PLAN TP JOIN REGION R ON(R.id_region=TP.id_region);
    END;
END Mantener_Tours;
/
CREATE OR REPLACE PACKAGE Mantener_Reserva
    AS
    PROCEDURE listar_reserva(Reservas OUT SYS_REFCURSOR);
    PROCEDURE actualizar_firma(identificador IN RESERVA.ID_RESERVA%TYPE, firma_func IN RESERVA.FIRMA%TYPE, R OUT INTEGER);
END Mantener_Reserva;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Reserva
    AS    
    PROCEDURE listar_reserva(Reservas OUT SYS_REFCURSOR)
        IS
    BEGIN
        OPEN Reservas FOR
            SELECT * FROM RESERVA JOIN CLIENTE USING(ID_CLIENTE) JOIN DEPARTAMENTO USING (ID_DPTO) WHERE TRANSPORTE <> 'N';
    END;
    PROCEDURE actualizar_firma(identificador IN RESERVA.ID_RESERVA%TYPE, firma_func IN RESERVA.FIRMA%TYPE, R OUT INTEGER)
    IS
    BEGIN
        UPDATE RESERVA 
        SET FIRMA = firma_func
        WHERE ID_RESERVA = identificador RETURNING 1 INTO R;
        IF R = 1 THEN
            UPDATE RESERVA 
            SET ESTADO_RESERVA = 'X'
            WHERE ID_RESERVA = identificador RETURNING 1 INTO R;
        END IF;
        IF r = 1 THEN
            COMMIT;
        END IF;
    END;    
END Mantener_Reserva;
/
CREATE OR REPLACE PACKAGE Mantener_Servicios_Dpto
    AS
    PROCEDURE insertar_svdpto(nombre IN SERVICIO.NOMBRE_SERV%TYPE, descripcion IN SERVICIO.DESC_SERV%TYPE, R OUT INTEGER);
    PROCEDURE actualizar_svdpto(identificador IN SERVICIO.ID_SERVICIO%TYPE, nombre IN SERVICIO.NOMBRE_SERV%TYPE,
        descripcion IN SERVICIO.DESC_SERV%TYPE, R OUT INTEGER);
    PROCEDURE eliminar_svdpto(identificador SERVICIO.ID_SERVICIO%TYPE, R OUT INTEGER);
    PROCEDURE listar_svdpto(Servicios_dpto OUT SYS_REFCURSOR);

END Mantener_Servicios_Dpto;
/
CREATE OR REPLACE PACKAGE BODY Mantener_Servicios_Dpto
    AS
    PROCEDURE insertar_svdpto(nombre IN SERVICIO.NOMBRE_SERV%TYPE, descripcion IN SERVICIO.DESC_SERV%TYPE, R OUT INTEGER)
    IS
        id_col rowid;
    BEGIN
        INSERT INTO SERVICIO(NOMBRE_SERV, DESC_SERV) VALUES(nombre, descripcion) RETURNING rowid INTO id_col;
        IF id_col IS NOT NULL THEN
            r:=1;
            COMMIT;
        END IF;
    END;
    PROCEDURE actualizar_svdpto(identificador IN SERVICIO.ID_SERVICIO%TYPE, nombre IN SERVICIO.NOMBRE_SERV%TYPE,
        descripcion IN SERVICIO.DESC_SERV%TYPE, R OUT INTEGER)
    IS
    BEGIN
        UPDATE SERVICIO 
            SET NOMBRE_SERV = nombre, DESC_SERV = descripcion
        WHERE ID_SERVICIO = identificador RETURNING 1 INTO R;
        IF r = 1 THEN
            COMMIT;
        END IF;
    END;
    PROCEDURE eliminar_svdpto(identificador SERVICIO.ID_SERVICIO%TYPE, R OUT INTEGER)
    IS 
    BEGIN 
        DELETE FROM SERVICIO WHERE ID_SERVICIO =  identificador RETURNING 1 INTO r;
        IF r = 1 THEN
            COMMIT;
        END IF;
    END;
    PROCEDURE listar_svdpto(Servicios_dpto OUT SYS_REFCURSOR)
    IS
    BEGIN
        OPEN Servicios_dpto FOR
            SELECT * FROM SERVICIO;
    END;
END Mantener_Servicios_Dpto;
/
DECLARE 
    r integer;
BEGIN
    Mantener_Usuario_Admin.Agregar_Admin('desktop@gmail.com', '123', 1235, '2-2', 'Test', 'Entrega2', r);
END;
/

DECLARE 
    R INTEGER;
BEGIN
    Mantener_Dpto.insertar_dpto('Transilvania', 59000, 'Franklin 231', 1, 5, 1, 0, R);
END;