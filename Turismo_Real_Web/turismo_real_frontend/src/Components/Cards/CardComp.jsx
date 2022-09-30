import Card from "react-bootstrap/Card";
import { Button, Col } from "react-bootstrap";
import { Row } from "react-bootstrap";

export const CardComponent = ({ NumeroDepto, capacidad, tarifa, direccion, comuna }) => {
  return (
    <>
      <Card className="card-style mx-auto mb-3">
        <Card.Body className="card-body">
          <Row>
            <Card.Header><h2>Departamento</h2></Card.Header>
            <Col>
              <br />
              <Card.Text><h4>Numero departamento: {NumeroDepto}</h4></Card.Text>
              <Card.Text><h4>Tarifa: {tarifa}</h4></Card.Text>
            </Col>
            <Col>
              <br />
              <Card.Text><h4>Direccion: {direccion}</h4></Card.Text>
              <Card.Text><h4>Comuna: {comuna}</h4></Card.Text>

            </Col>
            <Col>
              <br />
              <Card.Text><h4>Capacidad: {capacidad}</h4></Card.Text>
            </Col>
            <Col>
            <br />
            <Button>Reservar</Button>
            </Col>
          </Row>
        </Card.Body>
      </Card>
    </>
  );
};