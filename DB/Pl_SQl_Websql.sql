set serveroutput on 
/
CREATE OR REPLACE PACKAGE login_web AS
    PROCEDURE CREAR_USUARIO(email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEÑA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        rut IN CLIENTE.RUT_CLIENTE%TYPE, nombre IN CLIENTE.NOMBRES_CLIENTE%TYPE, apellido IN  CLIENTE.APELLIDOS_CLIENTE%TYPE, R OUT INT);
    PROCEDURE AUTENTIFICAR(email_aut IN USUARIO.email%type, psw_aut IN USUARIO.contraseña%type, R OUT INT);
END login_web;
/
CREATE OR REPLACE PACKAGE BODY login_web AS
    PROCEDURE CREAR_USUARIO(email_c IN USUARIO.EMAIL%TYPE, pass IN USUARIO.CONTRASEÑA%TYPE, fono IN USUARIO.TELEFONO%TYPE, 
        rut IN CLIENTE.RUT_CLIENTE%TYPE, nombre IN CLIENTE.NOMBRES_CLIENTE%TYPE, apellido IN  CLIENTE.APELLIDOS_CLIENTE%TYPE, R OUT INT) 
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
        /*Iniciar un punto de guardado, en caso de un error se vuelve a este punto*/
        SAVEPOINT A;
        
        /*Generar contraseña */
        v_pass:=GENERAR_CON(email_c, pass);
        INSERT INTO USUARIO(EMAIL, CONTRASEÑA, TELEFONO) VALUES(email_c, v_pass , fono) RETURNING rowid, ID_USUARIO INTO id_col, identificador_usr;
        /*Si el usuario fue creado, se inicia la creación de su rol*/ 
        IF id_col IS NOT NULL THEN
            /*Una vez creado su rol, se empareja con el usuario*/
            INSERT INTO CLIENTE(RUT_CLIENTE,NOMBRES_CLIENTE,APELLIDOS_CLIENTE,ID_USUARIO) VALUES(rut, nombre, apellido, identificador_usr) 
                RETURNING rowid,ID_CLIENTE INTO id_col,identificador_cli;
            IF id_col IS NOT NULL THEN
                UPDATE USUARIO SET ID_CLIENTE = identificador_cli WHERE ID_USUARIO = identificador_usr RETURNING 1 INTO r;
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
    PROCEDURE AUTENTIFICAR(email_aut IN USUARIO.email%type, psw_aut IN USUARIO.contraseña%type, R OUT INT)
    IS
        v_count number;
        v_pass VARCHAR2(40);
    BEGIN 
        /*Generar contraseña */
        v_pass:=GENERAR_CON(email_aut, psw_aut);
        SELECT cli.id_cliente INTO R
            FROM CLIENTE cli JOIN USUARIO USR ON(cli.id_usuario = usr.id_usuario)
                WHERE usr.email = email_aut and usr.contraseña = v_pass;
    EXCEPTION 
        WHEN No_Data_Found THEN
            R:= 0;
    END;
END login_web;
/

CREATE OR REPLACE PROCEDURE AGREGAR_RESERVA(idDepto IN RESERVA.ID_DPTO%TYPE, idCli RESERVA.ID_CLIENTE%TYPE,  estadoRes RESERVA.ESTADO_RESERVA%TYPE, 
        estadoPag RESERVA.ESTADO_PAGO%TYPE, checkIn RESERVA.CHECK_IN%TYPE, checkOut RESERVA.CHECK_OUT%TYPE, firmaRes RESERVA.FIRMA%TYPE, cant_acomp RESERVA.CANTIDAD_ACOMPAÑANTES%TYPE, transporte_reserva RESERVA.TRANSPORTE%TYPE, valorTotal RESERVA.VALOR_TOTAL%TYPE, R OUT INTEGER)
    IS
        id_col rowid;
        identificador_RES RESERVA.ID_RESERVA%TYPE;
        Reserva_Error_Ag EXCEPTION;
        PRAGMA EXCEPTION_INIT(Reserva_Error_Ag, -2101);
    BEGIN
        INSERT INTO RESERVA(ID_DPTO, ID_CLIENTE, ESTADO_RESERVA, ESTADO_PAGO, CHECK_IN, CHECK_OUT, FIRMA, CANTIDAD_ACOMPAÑANTES, TRANSPORTE, VALOR_TOTAL) 
            VALUES(idDepto, idCli, estadoRes, estadoPag, checkIn, checkOut, firmaRes, cant_acomp, transporte_reserva,    valorTotal) RETURNING rowid, ID_RESERVA INTO id_col, identificador_RES;
        /* Retornar un 1 si el insert fue correcto*/
        IF id_col IS NOT NULL THEN
            r:= identificador_RES;
            COMMIT;        
        /* Iniciar un error si no se ingresó*/
        ELSE 
            RAISE Reserva_Error_Ag;
        END IF;
    EXCEPTION
        WHEN Reserva_Error_Ag THEN
            R:= -2101;
    END;
/

/*Generar un usuario*/
DECLARE 
    r integer;
BEGIN
    login_web.CREAR_USUARIO('web@gmail.com','123',123,'1-1', 'Yerko','Basadont', r);
END;
/
SELECT NOMBRE_DPTO, CHECK_IN, CHECK_OUT, ESTADO_PAGO, VALOR_TOTAL FROM RESERVA RES JOIN DEPARTAMENTO DPTO USING(ID_DPTO);