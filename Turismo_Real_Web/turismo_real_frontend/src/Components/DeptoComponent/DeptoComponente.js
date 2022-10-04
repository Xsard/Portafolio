import React from "react";
import DeptoService from "../../services/DeptoService";
import { CardComponent } from "../Cards/CardComp";
import { Row } from "react-bootstrap";

class DeptoComponent extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            deptos: []
        }
    }

    componentDidMount() {
        DeptoService.getDeptos().then((Response) => {
            this.setState({ deptos: Response.data })
        });
    }

    render() {
        return (
            <>
                <center>
                    <Row className="mt-5 gx-0" style={{ width: "80%" }}>
                        {
                            this.state.deptos.map(
                                deptos =>
                                    <CardComponent
                                        NumeroDepto={deptos.nroDepto}
                                        capacidad={deptos.capacidad}
                                        tarifa={deptos.tarifaDiaria}
                                        direccion={deptos.direccion}
                                        comuna={deptos.idComuna}
                                    />
                            )
                        }
                    </Row>
                </center>

            </>
        );
    }
}

export default DeptoComponent