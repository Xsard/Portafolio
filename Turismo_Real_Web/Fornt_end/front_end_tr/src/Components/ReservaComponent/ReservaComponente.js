import React from "react";
import ReservaService from "../../services/ReservaService";
import axios from "axios";

const handleUpdate = async (id_reserva) => {
    const resp = await axios.post(`http://localhost:8080/api/v1/updateReserva/${id_reserva}`)
    console.log(resp.data)
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

                <div>
                    <h1 className="text-center">Reservas</h1>
                    <table className="table table-striped">
                        <thead>
                            <tr>
                                <td>Numero Reserva</td>
                                <td>Nombre departamento</td>
                                <td>Check in</td>
                                <td>Check out</td>
                                <td>Estado Pago</td>
                                <td>Valor total</td>
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
                                            <td>{Reserva.check_in}</td>
                                            <td>{Reserva.check_out}</td>
                                            <td>{Reserva.estado_pago}</td>
                                            <td>{Reserva.valor_total}</td>
                                            <td><button onClick={() => handleUpdate(Reserva.id_reserva)} className="btn btn-danger ">Cancelar</button></td>
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