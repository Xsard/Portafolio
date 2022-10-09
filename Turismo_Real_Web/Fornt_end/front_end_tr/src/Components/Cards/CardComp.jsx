import '../Cards/cards.css'
import img_test from '../../Img/depto_test.jpg'
import { Col } from 'react-bootstrap';

export const CardComponent = ({idDepto, NumeroDepto, capacidad, tarifa, direccion, comuna }) => {
  return (
    <>
    <Col>
      <div className="card mt-3">
        <img src={img_test} alt=".." className="card-img-top" />
        <div className="card-body text text-right">
          <h5>{direccion}</h5>
          <p className="card-text">
            Capacidad: {capacidad} <br />
            Tarifa: {tarifa} <br />
            Numero Departamento: {NumeroDepto} <br />
            Comuna: {comuna}  <br />
            <br />
            <a href={`/ReservaDepto/${idDepto}`} className="btn btn-primary"> Reservar</a>
          </p>
        </div>
      </div>
    </Col>
    </>
  );
};