import React from "react";
import TourServ from "../../services/TourService"
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import axios from "axios";

const handleContratar = async () => {
    let fecha = document.getElementById("fecha_tour").value;
    console.log (fecha)
}

const postTours = () => {
    const MySwal = withReactContent(Swal);

    MySwal.fire({
        title: "¿Desea agregar servicios extras?",
        icon: "info",
        showDenyButton: true,
        allowOutsideClick: false,
        confirmButtonText: 'Si',
        denyButtonText: 'No'
    }).then((respuesta) => {
        if (respuesta.isConfirmed) {
            const idReserva1 = localStorage.getItem('idReserva')
            window.location.replace(`/ListaServExtra/${idReserva1}`);
        } else {
            MySwal.fire({
                title: "Reserva Exitosa",
                icon: "success"
            }).then((respuesta) => {
                if (respuesta.isConfirmed) {
                    window.location.replace('/Inicio');

                }
            })
        }
    })
}
const PostCancelar = () => {
    const MySwal = withReactContent(Swal);

    MySwal.fire({
        title: "¿Desea continuar sin agregar un tour?",
        icon: "info",
        showDenyButton: true,
        allowOutsideClick: false,
        confirmButtonText: 'Si',
        denyButtonText: 'No'
    }).then((respuesta) => {
        if (respuesta.isConfirmed) {
            MySwal.fire({
                title: "¿Desea agregar servicios extras?",
                icon: "info",
                showDenyButton: true,
                allowOutsideClick: false,
                confirmButtonText: 'Si',
                denyButtonText: 'No'
            }).then((respuesta) => {
                if (respuesta.isConfirmed) {
                    const idReserva1 = localStorage.getItem('idReserva')
                    window.location.replace(`/ListaServExtra/${idReserva1}`);
                } else {
                    MySwal.fire({
                        title: "Reserva Exitosa",
                        icon: "success"
                    }).then((respuesta) => {
                        if (respuesta.isConfirmed) {
                            window.location.replace('/Inicio');

                        }
                    })
                }
            })
        } else {

        }
    })
}

class TourComponent extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            tours: []
        }
    }

    componentDidMount() {
        let id_res = localStorage.getItem('idReserva')
        TourServ.getTours(id_res).then((Response) => {
            this.setState({ tours: Response.data })
        });
    }

    render() {
        return (
            <>
                <div class="container">
                    <h1 className="text-center">Listado de tours</h1>
                    <table class="table table-fixed">
                        <thead class="table-dark">
                            <tr>
                                <td>Nombre del tour</td>
                                <td>Descripcion</td>
                                <td>Valor del tour</td>
                                <td></td>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                this.state.tours.map(
                                    tours =>
                                        <tr key={tours.id_tour}>
                                            <td>{tours.nombre_tour}</td>
                                            <td>{tours.desc_tour}</td>
                                            <td>{tours.valor_tour}</td>
                                            <td><button className="btn btn-primary" data-toggle="modal" data-target="#exampleModal">Contratar</button></td>
                                        </tr>
                                )
                            }
                        </tbody>
                    </table>
                    <br></br>
                    <div className="text text-center">
                        <button onClick={postTours} className="btn btn-primary">Continuar</button>&nbsp;<button className="btn btn-danger" onClick={PostCancelar}>Cancelar</button>
                    </div>
                </div>

                <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="exampleModalLabel">Seleccionar fecha del tour</h5>
                            </div>
                            <div class="modal-body">
                                <h5>Seleccionar fecha</h5>
                                <input type="date" id="fecha_tour"></input>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                                <button type="button" class="btn btn-primary" onClick={handleContratar}>Contratar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </>
        )
    }
}
export default TourComponent