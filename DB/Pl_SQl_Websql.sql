set serveroutput on 
/
CREATE OR REPLACE PACKAGE login_web AS
    FUNCTION CREAR_USUARIO(email_c USUARIO.EMAIL%TYPE, pass USUARIO.CONTRASEÑA%TYPE, fono USUARIO.TELEFONO%TYPE, rut CLIENTE.RUT_CLIENTE%TYPE, nombre CLIENTE.NOMBRES_CLIENTE%TYPE, apellido CLIENTE.APELLIDOS_CLIENTE%TYPE)
    RETURN INTEGER;
    FUNCTION AUTENTIFICAR(email_aut USUARIO.email%type, psw_aut USUARIO.contraseña%type)
    RETURN SYS_REFCURSOR;
END login_web;
/
CREATE OR REPLACE PACKAGE BODY login_web AS
    FUNCTION GENERAR_CON(p_email VARCHAR2, p_psw VARCHAR2)
    RETURN VARCHAR2
    IS
    BEGIN
        RETURN DBMS_CRYPTO.HASH(UTL_RAW.CAST_TO_RAW(UPPER(p_email) ||''|| UPPER(p_psw)),DBMS_CRYPTO.HASH_SH1);
    END;
    FUNCTION CREAR_USUARIO(email_c USUARIO.EMAIL%TYPE, pass USUARIO.CONTRASEÑA%TYPE, fono USUARIO.TELEFONO%TYPE, rut CLIENTE.RUT_CLIENTE%TYPE, nombre CLIENTE.NOMBRES_CLIENTE%TYPE, apellido CLIENTE.APELLIDOS_CLIENTE%TYPE)
    RETURN INTEGER
    IS 
        r integer;
        id_col rowid;
        identificador_usr USUARIO.ID_USUARIO%TYPE;
        identificador_cli CLIENTE.ID_CLIENTE%TYPE;
        v_pass VARCHAR2(40);
        error_crear_usuario EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_crear_usuario, -20001);
        error_crear_cliente EXCEPTION;
        PRAGMA EXCEPTION_INIT(error_crear_cliente, -20101);
    BEGIN 
        SAVEPOINT A;
        v_pass:=GENERAR_CON(email_c, pass);
        INSERT INTO USUARIO(EMAIL, CONTRASEÑA, TELEFONO) VALUES(email_c, v_pass , fono) RETURNING rowid, ID_USUARIO INTO id_col, identificador_usr;
        IF id_col IS NOT NULL THEN
            INSERT INTO CLIENTE(RUT_CLIENTE,NOMBRES_CLIENTE,APELLIDOS_CLIENTE,ID_USUARIO) VALUES(rut, nombre, apellido, identificador_usr) 
                RETURNING rowid,ID_CLIENTE INTO id_col,identificador_cli;
            IF id_col IS NOT NULL THEN
                UPDATE USUARIO SET ID_CLIENTE = identificador_cli WHERE ID_USUARIO = identificador_usr RETURNING 1 INTO r;
                IF r = 1 THEN
                    COMMIT;        
                END IF;
            ELSE 
                RAISE error_crear_cliente;
            END IF;
        ELSE
            RAISE error_crear_usuario;
        END IF;
        RETURN r;
    EXCEPTION
        WHEN DUP_VAL_ON_INDEX THEN
            ROLLBACK TO A;
            RETURN -1;
        WHEN error_crear_usuario THEN 
            ROLLBACK TO A;
            RETURN -20001;
        WHEN error_crear_cliente THEN
            ROLLBACK TO A;
            RETURN -20101;
    END;
        FUNCTION AUTENTIFICAR(email_aut USUARIO.email%type, psw_aut USUARIO.contraseña%type)
    RETURN SYS_REFCURSOR
    IS
    usr_con SYS_REFCURSOR;
    v_count number;
    v_pass VARCHAR2(40);
    BEGIN 
        v_pass:=GENERAR_CON(email_aut, psw_aut);
        OPEN usr_con FOR
            SELECT * 
            FROM CLIENTE cli JOIN USUARIO USR ON(cli.id_usuario = usr.id_usuario)
                WHERE usr.email = email_aut and usr.contraseña = v_pass;
        RETURN usr_con;
    EXCEPTION 
        WHEN No_Data_Found THEN
            DBMS_OUTPUT.PUT_LINE('EL USUARIO NO EXISTE');
            RETURN NULL;
    END;
END login_web;
