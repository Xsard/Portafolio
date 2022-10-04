import '../Cards/cards.css'
import img_test from '../../Img/depto_test.jpg'
import { Col } from 'react-bootstrap';

export const CardComponent = ({ NumeroDepto, capacidad, tarifa, direccion, comuna }) => {
  return (
    <>
    <Col>
      <div className="card mt-3">
        <img src={img_test} alt=".." class="card-img-top" />
        <div class="card-body text text-right">
          <h5>{direccion}</h5>
          <p class="card-text">
            Capacidad: {capacidad} <br />
            Tarifa: {tarifa} <br />
            Numero Departamento: {NumeroDepto} <br />
            Comuna: {comuna}  <br />
            <br />
            <a href="Inicio" class="btn btn-primary"> Reservar</a>
          </p>
        </div>
      </div>
    </Col>
    </>
  );
};