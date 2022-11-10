import React, { useState } from "react";
import serviciosServices from "../../services/ServExtra";
import "./DeptoComponente.css"
import { useParams } from "react-router-dom";
import axios from "axios";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";



const HandleInsertar = async (id_sv_extra) => {
    
    const MySwal = withReactContent(Swal);
    let reserva = localStorage.getItem("idReserva")
    let idCliente = localStorage.getItem("id_cliente")
    console.log(reserva)
    const resp = await axios.get(`http://localhost:8080/api/v1/traerDpto/${reserva}`)
    let id_depto1 = resp.data
    console.log(id_depto1)
    const resp1 = await axios.post("http://localhost:8080/api/v1/addservExtra", {
        id_reserva: reserva, id_svc_ex: id_sv_extra, id_dpto: id_depto1, id_cliente: idCliente
    })
    MySwal.fire({
        title: "Se ha agregado el servicio extra",
        icon: "success"
    })
}

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
            this.setState({ servicios: Response.data})
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
                                            {
                                                servicios.seleccionado === ''?
                                                <h1>a</h1>:
                                                <td><button  onClick={() => HandleInsertar(servicios.id_svc_ex)} className="btn btn-success ">Agregar</button></td>
                                                
                                            }
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