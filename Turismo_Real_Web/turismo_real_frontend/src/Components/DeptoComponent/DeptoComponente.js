import React from "react";
import DeptoService from "../../services/DeptoService";
import Button from 'react-bootstrap/Button';
import { CardComponent } from "../Cards/CardComp";
import {Row} from "react-bootstrap";

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
                <div>
                    <Row className="mx-auto mt-5" style={{ width: "80%" }}>
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
                </div>
            </>
        );
    }
}

export default DeptoComponent