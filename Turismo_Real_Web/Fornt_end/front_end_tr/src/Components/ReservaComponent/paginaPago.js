import React from "react"
import { useParams } from "react-router-dom";
import { Button, Form } from "react-bootstrap";
import { useState, useEffect } from "react";
import axios from "axios";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";

const Pago_web = () => {
    const { id_reserva } = useParams()
    const [valorTotal, setValorTotal] = useState('');

    useEffect(() => {
        const obtenerValorTotal = async () => {
            const resp = await axios.get(`http://localhost:8080/api/v1/obtenerReserva/${id_reserva}`)
            setValorTotal(resp.data.valor_total*(75/100))
        }
        obtenerValorTotal()
    }, [])

    const handleUpdate = async () => {
        const resp = await axios.post(`http://localhost:8080/api/v1/ActualizarPago/${id_reserva}`)
        console.log(resp.data.estado_pago)
    
        const MySwal = withReactContent(Swal);
        MySwal.fire({
            title: "el pago ha sido realizado con exito",
            icon: "success"
        }).then((respuesta) => {
            if (respuesta.isConfirmed) {
                window.location.replace('/ListaReserva');
            }
        })
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
                        >
                            <Form.Label>Numero Tarjeta</Form.Label>
                            <Form.Control type="text" placeholder="Ej: 2637 3627 4361 2163" />
                        </Form.Group>
                    </div>
                    <div className="form-row mb-3">
                        <Form.Group className="form-input mb-3"
                            type="text"
                            id="nombres"
                        >
                            <Form.Label>Nombre titular</Form.Label>
                            <Form.Control type="text" placeholder="Ingrese nombre del titular" />
                        </Form.Group>
                    </div>
                    <div className="form-row mb-3">
                        <Form.Group className="form-input mb-3"
                            type="text"
                            id="fecha"
                        >
                            <Form.Label>Fecha Expiración</Form.Label>
                            <Form.Control type="text" placeholder="Ej: 26/23" />
                        </Form.Group>
                    </div>
                    <div className="form-row mb-3">
                        <Form.Group className="form-input mb-3"
                            type="text"
                            id="cv"
                        >
                            <Form.Label>CV</Form.Label>
                            <Form.Control type="email" placeholder="Ej: 123" />
                        </Form.Group>
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