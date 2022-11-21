import { Col } from 'react-bootstrap';
import { Link } from 'react-router-dom';

//creamos la funcion que sera exportada para utilizar la card por defecto con los datos
export const CardServicio = ({ id_servicio, nombre_servicio, descripcion}) => {

  //devolvemos el codigo HTML con el estilo de card por defecto
  return (
    <>
      <Col>
        <div className="card mt-3" style={{background: "#222631", color: "#EEEEEE", borderColor: "#EEEEEE"}} >
          <div className="card-body text text-right">
            <h4>{nombre_servicio}</h4>
            <p className="card-text">
              Descripción: {descripcion} <br />
              <br />
              <br />
            </p>
          </div>
        </div>
      </Col>
    </>
  );
};