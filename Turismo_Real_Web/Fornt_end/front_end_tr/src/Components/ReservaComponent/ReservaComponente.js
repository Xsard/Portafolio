import React from "react";
import ReservaService from "../../services/ReservaService";
import axios from "axios";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import "../ReservaComponent/ReservaC.css"
import { Link } from "react-router-dom";

const handleUpdate = async (id_reserva) => {
    const resp = await axios.post(`http://localhost:8080/api/v1/updateReserva/${id_reserva}`)
    console.log(resp.data)

    const MySwal = withReactContent(Swal);
    MySwal.fire({
        title: "Su reserva ha sido cancelada",
        icon: "success"
    }).then((respuesta) => {
        if (respuesta.isConfirmed) {
            window.location.replace('/Inicio');
        }
    })
}

const handleAcompañantes = (id_reserva) => {
    localStorage.setItem('idReserva', id_reserva)
    let id_reserva1 = localStorage.getItem('idReserva')
    window.location.replace(`/editarAcompanantes/${id_reserva1}`);
}

const handleServExtra = (id_reserva) => {
    localStorage.setItem('idReserva', id_reserva)
    let id_reserva1 = localStorage.getItem('idReserva')
    window.location.replace(`/ListaServExtra/${id_reserva1}`);
}
const handlePago = (id_reserva) => {
    const MySwal = withReactContent(Swal);
    localStorage.setItem('idReserva', id_reserva)
    let id_reserva1 = localStorage.getItem('idReserva')
    MySwal.fire({
        title: "Una vez pagada la reserva no se puede editar ni agregar servicios extras, ¿Desea continuar?",
        icon: "warning",
        showDenyButton: true,
        confirmButtonText: 'Si',
        denyButtonText: 'No'
    }).then((respuesta) => {
        if (respuesta.isConfirmed) {
            window.location.replace(`/portalPago/${id_reserva1}`);
        }
    })

}

class ReservaComponente extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            Reserva: []
        }
    }

    componentDidMount() {
        let id_clienteLOL = localStorage.getItem("id_cliente")
        ReservaService.getReserva(id_clienteLOL).then((Response) => {
            this.setState({ Reserva: Response.data })

        });
    }

    render() {

        return (
            <>
                <div class="container">
                    <h1 className="text-center">Reservas</h1>
                    <table class="table table-fixed">
                        <thead class="table-dark">
                            <tr>
                                <td>Numero Reserva</td>
                                <td>Nombre departamento</td>
                                <td>Check in</td>
                                <td>Check out</td>
                                <td>Estado Pago</td>
                                <td>Valor total</td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                this.state.Reserva.map(
                                    Reserva =>
                                        <tr key={Reserva.id_reserva}>
                                            <td>{Reserva.id_reserva}</td>
                                            <td>{Reserva.nombre_dpto}</td>
                                            <td>{Reserva.check_in.slice(0, 10)}</td>
                                            <td>{Reserva.check_out.slice(0, 10)}</td>
                                            <td>
                                                {
                                                    Reserva.estado_pago === "A" ?
                                                        "Pago adelantado" :
                                                        "Pendiente"
                                                }

                                            </td>
                                            <td>{Reserva.valor_total}</td>
                                            <td>
                                                {
                                                    Reserva.estado_pago === "A" ?
                                                        <img src="https://icons.veryicon.com/png/o/miscellaneous/new-version-of-star-selected-icon/success-26.png"
                                                            height="35" width="35" alt="" /> :
                                                        <td><button onClick={() => handlePago(Reserva.id_reserva)} className="btn btn-success ">Pagar</button></td>
                                                }
                                            </td>
                                            <td>
                                                {
                                                    Reserva.estado_pago === "A" ?
                                                        <a></a> :
                                                        <td><button onClick={() => handleServExtra(Reserva.id_reserva)} className="btn btn-primary ">Editar Servicios Extras</button></td>
                                                }
                                            </td>
                                            <td>
                                                {
                                                    Reserva.estado_pago === "A" ?
                                                        <a></a> :
                                                        <td><button onClick={() => handleAcompañantes(Reserva.id_reserva)} className="btn btn-primary ">Editar reserva</button></td>
                                                }
                                            </td>
                                            {
                                                    Reserva.estado_pago === "A" ?
                                                        <a></a> :
                                                        <td><button onClick={() => handleUpdate(Reserva.id_reserva)} className="btn btn-danger ">Cancelar</button></td>
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

export default ReservaComponente