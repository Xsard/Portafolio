/*Uso de PL/SQL para la elimincaci�n de tablas*/
BEGIN
   FOR cur_rec IN (SELECT object_name, object_type
                   FROM user_objects
                   WHERE object_type IN
                             ('TABLE',
                              'VIEW',
                              'MATERIALIZED VIEW',
                              'PACKAGE',
                              'PROCEDURE',
                              'FUNCTION',
                              'SEQUENCE',
                              'SYNONYM',
                              'PACKAGE BODY'
                             ))
   LOOP
      BEGIN
         IF cur_rec.object_type = 'TABLE'
         THEN
            EXECUTE IMMEDIATE 'DROP '
                              || cur_rec.object_type
                              || ' "'
                              || cur_rec.object_name
                              || '" CASCADE CONSTRAINTS';
         ELSE
            EXECUTE IMMEDIATE 'DROP '
                              || cur_rec.object_type
                              || ' "'
                              || cur_rec.object_name
                              || '"';
         END IF;
      EXCEPTION
         WHEN OTHERS
         THEN
            DBMS_OUTPUT.put_line ('FAILED: DROP '
                                  || cur_rec.object_type
                                  || ' "'
                                  || cur_rec.object_name
                                  || '"'
                                 );
      END;
   END LOOP;
   FOR cur_rec IN (SELECT * 
                   FROM all_synonyms 
                   WHERE table_owner IN (SELECT USER FROM dual))
   LOOP
      BEGIN
         EXECUTE IMMEDIATE 'DROP PUBLIC SYNONYM ' || cur_rec.synonym_name;
      END;
   END LOOP;
END;
/

/*Creaci�n de tablas*/

/*Tablas de ubicaci�n*/
CREATE TABLE REGION(
    ID_REGION INTEGER,
    NOMBRE_REGION VARCHAR2(50) NOT NULL,
    CONSTRAINT REGION_PK PRIMARY KEY(ID_REGION)
);

CREATE TABLE COMUNA(
    ID_COMUNA INTEGER,
    NOMBRE_COMUNA VARCHAR2(50),
    ID_REGION INTEGER, 
    CONSTRAINT COMUNA_PK PRIMARY KEY(ID_COMUNA),
    CONSTRAINT FK_REGION_COMUNA FOREIGN KEY(ID_REGION) REFERENCES REGION(ID_REGION)
);

/*Tablas de roles y privilegios*/

CREATE TABLE USUARIO(
    ID_USUARIO INTEGER GENERATED BY DEFAULT AS IDENTITY,
    EMAIL VARCHAR2(254) NOT NULL,
    CONTRASE�A VARCHAR2(40) NOT NULL,
    TELEFONO INTEGER NOT NULL,
    ID_CLIENTE INTEGER,
    ID_ADMIN INTEGER,
    ID_FUNCIONARIO INTEGER,
    CONSTRAINT USUARIO_PK PRIMARY KEY(ID_USUARIO)
);

/*Se agrega la condici�n de exclusividad para los 3 tipos diferentes de usuario*/
ALTER TABLE USUARIO
    ADD CONSTRAINT ARC_PRIVILEGIO CHECK ( ( ( ID_ADMIN IS NOT NULL )
                                   AND ( ID_CLIENTE IS NULL )
                                   AND ( ID_FUNCIONARIO IS NULL ) )
                                 OR ( ( ID_CLIENTE IS NOT NULL )
                                      AND ( ID_ADMIN IS NULL )
                                      AND ( ID_FUNCIONARIO IS NULL ) )
                                 OR ( ( ID_FUNCIONARIO IS NOT NULL )
                                      AND ( ID_ADMIN IS NULL )
                                      AND ( ID_CLIENTE IS NULL ) )
                                OR ( ID_FUNCIONARIO IS NULL )
                                      AND ( ID_ADMIN IS NULL )
                                      AND ( ID_CLIENTE IS NULL ));
                                      
/*Craci�n de index para relaciones 1-1 de la tabla Usuario*/                                      
CREATE UNIQUE INDEX USER_ADMIN ON
    USUARIO(
        ID_ADMIN ASC
    );
    
CREATE UNIQUE INDEX USER_CLI ON
    USUARIO(
        ID_CLIENTE ASC
    );
    
CREATE UNIQUE INDEX USER_FUNC ON
    USUARIO(
        ID_FUNCIONARIO ASC
    );

/*Creaci�n index para el campo �nico Email*/
CREATE UNIQUE INDEX USER_EMAIL ON
    USUARIO(
        EMAIL ASC
    );

/*Creaci�n index para el campo �nico Tel�fono*/
CREATE UNIQUE INDEX USER_TELEFONO ON
    USUARIO(
        TELEFONO ASC
    );

/*Creaci�n de Rol Cliente*/
CREATE TABLE CLIENTE(
    ID_CLIENTE INTEGER GENERATED BY DEFAULT AS IDENTITY,
    RUT_CLIENTE VARCHAR2(10) NOT NULL,
    NOMBRES_CLIENTE VARCHAR2(60) NOT NULL,
    APELLIDOS_CLIENTE VARCHAR2(60) NOT NULL,
    ID_USUARIO INTEGER NOT NULL,
    CONSTRAINT CLIENTE_PK PRIMARY KEY(ID_CLIENTE),
    CONSTRAINT FK_CLIENTE_USR FOREIGN KEY(ID_USUARIO) REFERENCES USUARIO(ID_USUARIO)
);

/*Relaci�n 1-1 entre Cliente y Usuario*/
CREATE UNIQUE INDEX CLIENTE_USR ON
    CLIENTE(
        ID_USUARIO ASC
    );

/*Rut como valor �nico*/
CREATE UNIQUE INDEX CLIENTE_RUT ON
    CLIENTE(
        RUT_CLIENTE ASC
    );

/*Creaci�n de Rol Funcionario*/
CREATE TABLE FUNCIONARIO(
    ID_FUNCIONARIO INTEGER GENERATED BY DEFAULT AS IDENTITY,
    RUT_FUNCIONARIO VARCHAR2(10) NOT NULL,
    NOMBRES_FUNCIONARIO VARCHAR2(60) NOT NULL,
    APELLIDOS_FUNCIONARIO VARCHAR2(60) NOT NULL,
    ID_USUARIO INTEGER NOT NULL,
    CONSTRAINT FUNCIONARIO_PK PRIMARY KEY(ID_FUNCIONARIO),
    CONSTRAINT FK_FUNCIONARIO_USR FOREIGN KEY(ID_USUARIO) REFERENCES USUARIO(ID_USUARIO)
);

/*Relaci�n 1-1 entre Funcionario y Usuario*/
CREATE UNIQUE INDEX FUNCIONARIO_USR ON
    FUNCIONARIO(
        ID_USUARIO ASC
    );

/*Rut como valor �nico*/
CREATE UNIQUE INDEX FUNCIONARIO_RUT ON
    FUNCIONARIO(
        RUT_FUNCIONARIO ASC
    );

/*Creaci�n de Rol Administrador*/
CREATE TABLE ADMINISTRADOR(
    ID_ADMIN INTEGER GENERATED BY DEFAULT AS IDENTITY,
    RUT_ADMIN VARCHAR2(10) NOT NULL,
    NOMBRES_ADMIN VARCHAR2(60) NOT NULL,
    APELLIDOS_ADMIN VARCHAR2(60) NOT NULL,
    ID_USUARIO INTEGER NOT NULL,
    CONSTRAINT ADMIN_PK PRIMARY KEY(ID_ADMIN),
    CONSTRAINT FK_ADMIN_USR FOREIGN KEY(ID_USUARIO) REFERENCES USUARIO(ID_USUARIO)
);

/*Relaci�n 1-1 entre Administrador y Usuario*/
CREATE UNIQUE INDEX ADMIN_USR ON
    ADMINISTRADOR(
        ID_USUARIO ASC
    );

/*Rut como valor �nico*/
CREATE UNIQUE INDEX ADMIN_RUT ON
    ADMINISTRADOR(
        RUT_ADMIN ASC
    );
    
/*Implementaci�n de las Foreign Keys para la tabla Usuario*/
ALTER TABLE USUARIO
    ADD CONSTRAINT FK_USUARIO_CLI FOREIGN KEY(ID_CLIENTE) REFERENCES CLIENTE(ID_CLIENTE);

ALTER TABLE USUARIO
    ADD CONSTRAINT FK_USUARIO_ADMIN FOREIGN KEY(ID_ADMIN) REFERENCES ADMINISTRADOR(ID_ADMIN);

ALTER TABLE USUARIO
    ADD CONSTRAINT FK_USUARIO_FUNC FOREIGN KEY(ID_FUNCIONARIO) REFERENCES FUNCIONARIO(ID_FUNCIONARIO);

/*Tablas de informaci�n del deparatamento*/

/*Creaci�n tabla departamento*/
CREATE TABLE DEPARTAMENTO(
    ID_DPTO INTEGER GENERATED BY DEFAULT AS IDENTITY, 
    TARIFA_DIARIA NUMBER NOT NULL, 
    DIRECCION VARCHAR2(20) NOT NULL,
    NRO_DPTO INTEGER NOT NULL,
    CAPACIDAD INTEGER NOT NULL,
    ID_COMUNA INTEGER NOT NULL,
    CONSTRAINT DEPARTAMENTO_PK PRIMARY KEY(ID_DPTO),
    CONSTRAINT DEPTO_COMUNA_FK FOREIGN KEY(ID_COMUNA) REFERENCES COMUNA(ID_COMUNA)
);

/*Creaci�n tabla fotograf�as departamento*/
CREATE TABLE FOTOGRAFIA_DPTO(
    ID_FOTO INTEGER GENERATED BY DEFAULT AS IDENTITY, 
    ID_DPTO INTEGER,
    FOTO_PATH VARCHAR2(100) NOT NULL,
    ALT_FOTO VARCHAR2(50) NOT NULL,
    CONSTRAINT FOTO_DPTO_PK PRIMARY KEY(ID_FOTO, ID_DPTO),
    CONSTRAINT FOTO_DEPTO_FK FOREIGN KEY(ID_DPTO) REFERENCES DEPARTAMENTO(ID_DPTO)
);

/*El path de la fotograf�a debe ser �nico para cada una de estas*/
CREATE UNIQUE INDEX FOTO_PATH ON
    FOTOGRAFIA_DPTO(
        FOTO_PATH ASC
    );
    
/*Creaci�n tabla inventario*/
CREATE TABLE INVENTARIO_DPTO(
    ID_INV INTEGER GENERATED BY DEFAULT AS IDENTITY,
    ID_DPTO INTEGER,
    NOMBRE_OBJETO VARCHAR2(50) NOT NULL,
    CANT_OBJETO INTEGER DEFAULT 0 NOT NULL,
    VALOR_UNITARIO_OBJ NUMBER NOT NULL,
    CONSTRAINT INVENTARIO_DPTO_PK PRIMARY KEY(ID_INV, ID_DPTO),
    CONSTRAINT INVENTARIO_DPTO_FK FOREIGN KEY(ID_DPTO) REFERENCES DEPARTAMENTO(ID_DPTO)
    ON DELETE CASCADE
);

/*Creaci�n tabla de servicios de un departamento*/
CREATE TABLE SERVICIO(
    ID_SERVICIO INTEGER GENERATED BY DEFAULT AS IDENTITY,
    NOMBRE_SERV VARCHAR2(50) NOT NULL,
    DESC_SERV VARCHAR2(1000) NOT NULL,
    CONSTRAINT SERVICIO_PK PRIMARY KEY(ID_SERVICIO)    
);

/*Creaci�n tabla servicio_dpto*/
CREATE TABLE SERVICIO_DPTO(
    ID_SERVICIO INTEGER,
    ID_DPTO INTEGER,
    ESTADO_SERVICIO CHAR(1),
    CONSTRAINT SERVICIO_DEPTO_PK PRIMARY KEY(ID_SERVICIO, ID_DPTO),
    CONSTRAINT DEPTO_SERVICIO_FK FOREIGN KEY(ID_DPTO) REFERENCES SERVICIO(ID_SERVICIO),
    CONSTRAINT SERVICIO_DEPTO_FK FOREIGN KEY(ID_DPTO) REFERENCES DEPARTAMENTO(ID_DPTO)
);

/*Creaci�n tabla mantenimiento*/
CREATE TABLE MANTENIMIENTO(
    ID_MANT INTEGER GENERATED BY DEFAULT AS IDENTITY,
    ID_DPTO INTEGER NOT NULL,
    NOMBRE_MANT VARCHAR2(50) NOT NULL,
    DESC_MANT VARCHAR2(2000) NOT NULL,
    FECHA_INICIO TIMESTAMP NOT NULL,
    FECHAR_TERMINO TIMESTAMP NOT NULL,
    ESTADO CHAR(1) NOT NULL,
    COSTO_MANTENCION NUMBER NOT NULL,
    CONSTRAINT MANTENIMIENTO_PK PRIMARY KEY(ID_MANT,ID_DPTO),
    CONSTRAINT MANTENIMIENTO_DPTO_FK FOREIGN KEY(ID_DPTO) REFERENCES DEPARTAMENTO(ID_DPTO)
);

/*Tablas relacionadas a la reserva*/

/*Creacion tabla reserva*/
CREATE TABLE RESERVA(
    ID_RESERVA INTEGER GENERATED BY DEFAULT AS IDENTITY, 
    ID_DPTO INTEGER NOT NULL,
    ID_CLIENTE INTEGER NOT NULL,
    ESTADO_RESERVA CHAR(1) DEFAULT 'I' NOT NULL,
    ESTADO_PAGO CHAR(1) DEFAULT 'P' NOT NULL,
    CHECK_IN TIMESTAMP NOT NULL,
    CHECK_OUT TIMESTAMP NOT NULL,
    FIRMA CHAR(1) DEFAULT 0 NOT NULL,
    VALOR_TOTAL NUMBER NOT NULL,
    CONSTRAINT RESERVA_PK PRIMARY KEY(ID_RESERVA, ID_DPTO, ID_CLIENTE),
    CONSTRAINT RESERVA_CLI_FK FOREIGN KEY(ID_CLIENTE) REFERENCES CLIENTE(ID_CLIENTE), 
    CONSTRAINT RESERVA_RESERVA_FK FOREIGN KEY(ID_DPTO) REFERENCES DEPARTAMENTO(ID_DPTO) 
);

/*Creacion tabla tours*/
CREATE TABLE TOUR_PLAN(
    ID_TOUR INTEGER GENERATED BY DEFAULT AS IDENTITY, 
    NOMBRE_TOUR VARCHAR2(50) NOT NULL,
    DESC_TOUR VARCHAR2(2000) NOT NULL,
    VALOR_TOUR NUMBER NOT NULL,
    ID_REGION INTEGER NOT NULL,
    CONSTRAINT TOUR_PLAN_PK PRIMARY KEY(ID_TOUR),
    CONSTRAINT TOUR_PLAN_REGION_FK FOREIGN KEY(ID_REGION) REFERENCES REGION(ID_REGION)
);

/*Creacion de la tabla intermedia entre tour y reserva*/
CREATE TABLE TOUR_RESERVA(
    ID_RESERVA INTEGER,
    ID_TOUR INTEGER,
    FECHA_TOUR TIMESTAMP NOT NULL,
    ID_DPTO INTEGER NOT NULL,
    ID_CLIENTE INTEGER NOT NULL,
    CONSTRAINT TOUR_RESERVA_PK PRIMARY KEY(ID_TOUR, ID_RESERVA),
    CONSTRAINT TOUR_TOUR_PLAN_FK FOREIGN KEY(ID_TOUR) REFERENCES TOUR_PLAN(ID_TOUR),
    CONSTRAINT TOUR_RESERVA_FK FOREIGN KEY(ID_RESERVA, ID_DPTO, ID_CLIENTE) REFERENCES RESERVA(ID_RESERVA, ID_DPTO, ID_CLIENTE)
);

/*Creacion tabla servicios_extra*/
CREATE TABLE SERVICIO_EXTRA(
    ID_SVC_EX INTEGER GENERATED BY DEFAULT AS IDENTITY,
    NOMBRE_SERV_EX VARCHAR2(50) NOT NULL,
    DESC_SERV_EX VARCHAR2(1000) NOT NULL,
    VALOR_SERV_EX NUMBER NOT NULL,
    CONSTRAINT SERVICIO_EXTRA_PK PRIMARY KEY(ID_SVC_EX) 
);

/*Creacion de la tabla intermedia entre reserva y servicios_extra*/
CREATE TABLE RESERVA_SERVICIOS_EXTRAS(
    ID_RESERVA INTEGER,
    ID_SVC_EX INTEGER,
    ID_DPTO INTEGER NOT NULL,
    ID_CLIENTE INTEGER NOT NULL,
    CONSTRAINT RESER_SVR_EX_PK PRIMARY KEY(ID_SVC_EX, ID_RESERVA),
    CONSTRAINT RESERVA_SRV_EX_FK FOREIGN KEY(ID_RESERVA, ID_DPTO, ID_CLIENTE) REFERENCES RESERVA(ID_RESERVA, ID_DPTO, ID_CLIENTE),
    CONSTRAINT SERVICIOS_EXTRAS_RESER_FK FOREIGN KEY(ID_SVC_EX) REFERENCES SERVICIO_EXTRA(ID_SVC_EX)
);

/*Creacion tabla plan_transporte*/
CREATE TABLE PLAN_TRANSPORTE(
    ID_PLAN_TRANS INTEGER GENERATED BY DEFAULT AS IDENTITY,
    EMPRESA_TRANS VARCHAR2(250) NOT NULL,
    CANT_MIN_TRANS INTEGER NOT NULL,
    CANT_MAX_TRANS INTEGER NOT NULL,
    VALOR NUMBER NOT NULL,
    CONSTRAINT PLAN_TRANSPORTE_FK PRIMARY KEY(ID_PLAN_TRANS)
);

/*Creacion tabla intermedia entre servicio_transporte*/
CREATE TABLE SERVICIO_TRANSPORTE (
    ID_TRANSPORTE INTEGER GENERATED BY DEFAULT AS IDENTITY,
    ID_RESERVA INTEGER,
    ID_PLAN_TRANS INTEGER, 
    LUGAR VARCHAR2(50) NOT NULL,
    FECHA TIMESTAMP NOT NULL,
    ID_DPTO INTEGER,
    ID_CLIENTE INTEGER,
    CONSTRAINT SERVICIO_TRANSPORTE_PK PRIMARY KEY(ID_TRANSPORTE, ID_RESERVA),
    CONSTRAINT P_TRANS_RESERVA_FK FOREIGN KEY(ID_TRANSPORTE) REFERENCES PLAN_TRANSPORTE(ID_PLAN_TRANS),
    CONSTRAINT TRAN_S_RESERVA_FK FOREIGN KEY(ID_RESERVA, ID_DPTO, ID_CLIENTE) REFERENCES RESERVA(ID_RESERVA, ID_DPTO, ID_CLIENTE)
);

/*Creacion tabla acompa�ante*/
CREATE TABLE ACOMPA�ANTE(
    ID_ACOMPA�ANTE INTEGER GENERATED BY DEFAULT AS IDENTITY,
    ID_RESERVA INTEGER,
    NOMBRE_ACM VARCHAR2(60) NOT NULL,
    APELLIDOS_ACM VARCHAR2(60) NOT NULL,
    ID_DPTO INTEGER,
    ID_CLIENTE INTEGER,
    CONSTRAINT ACM_PK PRIMARY KEY(ID_ACOMPA�ANTE, ID_RESERVA),
    CONSTRAINT ACM_RESERVA_FK FOREIGN KEY(ID_RESERVA, ID_DPTO, ID_CLIENTE) REFERENCES RESERVA(ID_RESERVA, ID_DPTO, ID_CLIENTE)
);

/*Creacion de checklist*/
CREATE TABLE MULTA(
    ID_MULTA INTEGER GENERATED BY DEFAULT AS IDENTITY,
    ID_RESERVA INTEGER,
    VALOR NUMBER NOT NULL,
    RAZON_MULTA VARCHAR2(50) NOT NULL,
    DESC_MULTA VARCHAR2(200) NOT NULL,
    ESTADO_PAGO CHAR(1) DEFAULT 0 NOT NULL,
    ID_DPTO INTEGER,
    ID_CLIENTE INTEGER,
    CONSTRAINT MULTA_PK PRIMARY KEY(ID_MULTA, ID_RESERVA),
    CONSTRAINT MULTA_RESERVA_FK FOREIGN KEY(ID_RESERVA, ID_DPTO, ID_CLIENTE) REFERENCES RESERVA(ID_RESERVA, ID_DPTO, ID_CLIENTE)
);
    
/*Regiones y comunas*/
INSERT INTO Region VALUES (1,'Arica y Parinacota');
INSERT INTO Region VALUES (2,'Tarapac�');
INSERT INTO Region VALUES (3,'Antofagasta');
INSERT INTO Region VALUES (4,'Atacama');
INSERT INTO Region VALUES (5,'Coquimbo');
INSERT INTO Region VALUES (6,'Valpara�so');
INSERT INTO Region VALUES (7,'Metropolitana de Santiago');
INSERT INTO Region VALUES (8,'Libertador General Bernardo OHiggins');
INSERT INTO Region VALUES (9,'Maule');
INSERT INTO Region VALUES (10,'�uble');
INSERT INTO Region VALUES (11,'Biob�o');
INSERT INTO Region VALUES (12,'La Araucan�a');
INSERT INTO Region VALUES (13,'Los R�os');
INSERT INTO Region VALUES (14,'Los Lagos');
INSERT INTO Region VALUES (15,'Ays�n del General Carlos Ib��ez del Campo');
INSERT INTO Region VALUES (16,'Magallanes y la Ant�rtica Chilena');

INSERT INTO Comuna VALUES (1,'Arica', 1);
INSERT INTO Comuna VALUES (2,'Iquique', 2);
INSERT INTO Comuna VALUES (3,'San Pedro de Atacama', 3);
INSERT INTO Comuna VALUES (4,'Copiap�', 4);
INSERT INTO Comuna VALUES (5,'La Serena', 5);
INSERT INTO Comuna VALUES (6,'Vi�a del Mar', 6);
INSERT INTO Comuna VALUES (7,'Santiago', 7);
INSERT INTO Comuna VALUES (8,'Pichilemu', 8);
INSERT INTO Comuna VALUES (9,'Curic�', 9);
INSERT INTO Comuna VALUES (10,'Chill�n', 10);
INSERT INTO Comuna VALUES (11,'Concepci�n', 11);
INSERT INTO Comuna VALUES (12,'Puc�n', 12);
INSERT INTO Comuna VALUES (13,'Valdivia', 13);
INSERT INTO Comuna VALUES (14,'Puerto Varas', 14);
INSERT INTO Comuna VALUES (15,'Ays�n', 15);
INSERT INTO Comuna VALUES (16,'Punta Arenas', 16);
/**/
INSERT INTO Usuario VALUES (1, 'test@gmail.com', 'test123', 123456789, null, null, null);
INSERT INTO Administrador values(1, '20441631-1', 'Test', 'Iteraci�n', 1);
UPDATE Usuario SET id_admin = 1 WHERE id_usuario = 1;
INSERT INTO DEPARTAMENTO VALUES(9,1,1,1,1,1);
COMMIT;