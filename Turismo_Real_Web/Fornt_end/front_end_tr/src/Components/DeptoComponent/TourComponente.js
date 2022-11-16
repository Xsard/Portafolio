import React from "react";
import TourServ from "../../services/TourService"
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";

const postTours= () => {
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
                                            <td><button className="btn btn-primary">Contratar</button></td>
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
            </>
        )
    }
}
export default TourComponent