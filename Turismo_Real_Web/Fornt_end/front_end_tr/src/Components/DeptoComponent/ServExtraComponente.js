import React from "react";
import serviciosServices from "../../services/ServExtra";
import { CardComponent } from "../Cards/CardComp";
import { Row } from "react-bootstrap";
import "./DeptoComponente.css"
class ServExtraComponente extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            servicios: []
        }
    }
    //llamar al localStorage
    componentDidMount() {
        serviciosServices.getServExtra().then((Response) => {
            this.setState({ servicios: Response.data })
        });
    }

    render() {
        return (
            <>
                <div class="container">
                    <h1 className="text-center">Listado de servicios extras</h1>
                    <table class="table table-fixed">
                        <thead class="table-dark">
                            <tr>
                                <td>Nombre del Servicio</td>
                                <td>Descripcion</td>
                                <td>Valor del servicio</td>
                                <td></td>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                this.state.servicios.map(
                                    servicios =>
                                        <tr key={servicios.id_svc_ex}>
                                            <td>{servicios.nombre_serv_ex}</td>
                                            <td>{servicios.desc_serv_ex}</td>
                                            <td>{servicios.valor_serv_ex}</td>
                                            <td><button className="btn btn-success ">Agregar</button></td>
                                        </tr>
                                )
                            }
                        </tbody>
                    </table>
                </div>
            </>
        );
    }
}

export default ServExtraComponente