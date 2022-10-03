import '../Cards/cards.css'
import img_test from '../../Img/depto_test.jpg'
import "../Cards/cards.css"

export const CardComponent = ({ NumeroDepto, capacidad, tarifa, direccion, comuna }) => {
  return (
    <>
        <div className="card mx-3 mt-3" style={{ width: "27rem" }}>
          <img src={img_test} alt=".." class="card-img-top" />
          <div class="card-body text text-right">
            <h5>{direccion}</h5>
            <p class="card-text">
              Capacidad: {capacidad} <br />
              Tarifa: {tarifa} <br />
              Numero Departamento: {NumeroDepto} <br />
              Comuna: {comuna}  <br />
              <br />
              <a href="#" class="btn btn-primary"> Reservar</a>
            </p>
          </div>
        </div>
    </>
  );
};