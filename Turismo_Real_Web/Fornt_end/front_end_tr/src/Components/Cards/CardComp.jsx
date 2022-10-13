import '../Cards/cards.css'
import { Col } from 'react-bootstrap';
import axios from "axios";


export const CardComponent = ({ idDepto, NumeroDepto, capacidad, tarifa, direccion, comuna, foto_path }) => {

  const handlepolla = () => {
    const resp = axios.get("http://localhost:8080/api/v1/id_foto")
    console.log(resp.data)
  }
  return (
    <>
      <Col>
        <div className="card mt-3" >
          <img src={""} alt={foto_path} className="card-img-top" />
          <div className="card-body text text-right">
            <h5>{direccion}</h5>
            <p className="card-text">
              Capacidad: {capacidad} <br />
              Tarifa: {tarifa} <br />
              Numero Departamento: {NumeroDepto} <br />
              Comuna: {comuna}  <br />
              <br />
              <a onClick={handlepolla} className="btn btn-primary"> Reservar</a>
            </p>
          </div>
        </div>
      </Col>
    </>
  );
};
