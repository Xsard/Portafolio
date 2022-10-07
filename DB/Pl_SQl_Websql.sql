set serveroutput on 
/
CREATE OR REPLACE PACKAGE login_web AS
    PROCEDURE CREAR_USUARIO(email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEŅA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        rut IN CLIENTE.RUT_CLIENTE%TYPE, nombre IN CLIENTE.NOMBRES_CLIENTE%TYPE, apellidoIN  CLIENTE.APELLIDOS_CLIENTE%TYPE, R OUT INT);
    PROCEDURE AUTENTIFICAR(email_aut IN USUARIO.email%type, psw_aut IN USUARIO.contraseņa%type, R OUT INT);
END login_web;
/
CREATE OR REPLACE PACKAGE BODY login_web AS
    PROCEDURE CREAR_USUARIO(email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEŅA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        rut IN CLIENTE.RUT_CLIENTE%TYPE, nombre IN CLIENTE.NOMBRES_CLIENTE%TYPE, apellidoIN  CLIENTE.APELLIDOS_CLIENTE%TYPE, R OUT INT) 
    IS 
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
        INSERT INTO USUARIO(EMAIL, CONTRASEŅA, TELEFONO) VALUES(email_c, v_pass , fono) RETURNING rowid, ID_USUARIO INTO id_col, identificador_usr;
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
    EXCEPTION
        WHEN DUP_VAL_ON_INDEX THEN
            ROLLBACK TO A;
            R:= -1;
        WHEN error_crear_usuario THEN 
            ROLLBACK TO A;
            R:= -20001;
        WHEN error_crear_cliente THEN
            ROLLBACK TO A;
            R:= -20101;
    END;
    PROCEDURE AUTENTIFICAR(email_aut IN USUARIO.email%type, psw_aut IN USUARIO.contraseņa%type, R OUT INT)
    IS
    v_count number;
    v_pass VARCHAR2(40);
    BEGIN 
        v_pass:=GENERAR_CON(email_aut, psw_aut);
        OPEN usr_con FOR
            SELECT * 
            FROM CLIENTE cli JOIN USUARIO USR ON(cli.id_usuario = usr.id_usuario)
                WHERE usr.email = email_aut and usr.contraseņa = v_pass;
    EXCEPTION 
        WHEN No_Data_Found THEN
            DBMS_OUTPUT.PUT_LINE('EL USUARIO NO EXISTE');
            R:= NULL;
    END;
END login_web;

