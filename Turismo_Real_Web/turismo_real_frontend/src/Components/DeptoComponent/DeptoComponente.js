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
                <div className="mx-auto col-9">
                    <h1 className="text-center"> Departamentos</h1>
                    <table className="table table-striped ">
                        <thead>
                            <tr>
                                <td>Numero Departamento</td>
                                <td>Capacidad</td>
                                <td>Tarifa diaria</td>
                                <td>Direccion</td>
                                <td>Comuna</td>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                this.state.deptos.map(
                                    deptos =>
                                        <tr key={deptos.idDepto}>
                                            <td>{deptos.nroDepto}</td>
                                            <td>{deptos.capacidad}</td>
                                            <td>{deptos.tarifaDiaria}</td>
                                            <td>{deptos.direccion}</td>
                                            <td>{deptos.idComuna}</td>
                                            <td><Button variant="primary" type="submit" className='text mx-3' href='Registrarse'>
                                                Reservar
                                            </Button></td>
                                        </tr>
                                )
                            }
                        </tbody>
                    </table>

                </div>
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