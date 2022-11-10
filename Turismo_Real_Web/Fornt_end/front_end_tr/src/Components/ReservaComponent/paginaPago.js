import React from "react"
import { useParams } from "react-router-dom";
import { Button, Form } from "react-bootstrap";
import { useState, useEffect } from "react";
import axios from "axios";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import "../ReservaComponent/ReservaC.css"

const Pago_web = () => {
    const { id_reserva } = useParams()
    const [valorTotal, setValorTotal] = useState('');
    const [tarjeta, setTarjeta] = useState('');
    const [nombres, setNombres] = useState('');
    const [fecha, setFecha] = useState('');
    const [fecha1, setFecha1] = useState('');
    const [cv, setCv] = useState('');
    const MySwal = withReactContent(Swal);

    useEffect(() => {
        const obtenerValorTotal = async () => {
            const resp = await axios.get(`http://localhost:8080/api/v1/obtenerReserva/${id_reserva}`)
            setValorTotal(resp.data.valor_total * (75 / 100))
        }
        obtenerValorTotal()
    })


    const handleUpdate = async () => {

        if (tarjeta === '' || nombres === '' || fecha === '' || cv === '' || fecha1 === '') {
            MySwal.fire({
                title: "Debe rellenar los campos solicitados",
                icon: "error"
            })
        }
        else {
            const resp = await axios.post(`http://localhost:8080/api/v1/ActualizarPago/${id_reserva}`)
            console.log(resp.data.estado_pago)

            
            MySwal.fire({
                title: "el pago ha sido realizado con exito",
                icon: "success"
            }).then((respuesta) => {
                if (respuesta.isConfirmed) {
                    window.location.replace('/ListaReserva');
                }
            })
        }
    }

    return (
        <>
            <div className="mx-auto">
                <br></br>
                <h2 className="text-center">Portal de pagos Turismo Real</h2>
                <div className="mx-auto mt-5 w-25" >
                    <div className="form-row mb-3">
                        <Form.Group className="form-input mb-3"
                            type="text"
                            id="numerotarjeta"
                            value={tarjeta}
                            onChange={(e) => setTarjeta(e.target.value)}
                        >
                            <Form.Label>Numero Tarjeta</Form.Label>
                            <Form.Control type="text" placeholder="Ej: 2637 3627 4361 2163" maxLength={16}/>
                        </Form.Group>
                    </div>
                    <div className="form-row mb-3">
                        <Form.Group className="form-input mb-3"
                            type="text"
                            id="nombres"
                            value={nombres}
                            onChange={(e) => setNombres(e.target.value)}
                        >
                            <Form.Label>Nombre titular</Form.Label>
                            <Form.Control type="text" placeholder="Ingrese nombre del titular" />
                        </Form.Group>
                    </div>
                    <br></br>
                    <div className="form-row mb-3">
                        <label>Fecha</label>&nbsp;&nbsp;
                        <p class="fecha"><input className="form-control" type="text" style={{width : "55px", heigth : "30px"}} 
                        value={fecha}
                        onChange={(e) => setFecha(e.target.value)}
                        maxLength={2}></input></p>&nbsp;
                        <h5 class="fecha">/</h5>&nbsp;
                        <p class="fecha"><input className="form-control" type="text" style={{width : "55px", heigth : "30px"}}
                        value={fecha1}
                        onChange={(e) => setFecha1(e.target.value)}
                        maxLength={2}></input></p>&nbsp;&nbsp;
                        <label>CV</label>&nbsp;&nbsp;
                        <p class="fecha"><input className="form-control" type="text" style={{width : "65px", heigth : "30px"}} 
                        value={cv}
                        onChange={(e) => setCv(e.target.value)}
                        maxLength={3}></input></p>&nbsp;
                    </div>   
                    <br></br>
                    <h2>El valor total a pagar es: {valorTotal}</h2>
                    <br></br>
                    <Button variant="primary" onClick={handleUpdate}>
                        Pagar
                    </Button>
                </div>
            </div>
        </>
    );
}
export default Pago_web;