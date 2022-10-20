package com.turismo.backend_turismo_real.controlador;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.turismo.backend_turismo_real.modelo.Reserva;
import com.turismo.backend_turismo_real.service.ReservaServImplement;

@RestController
@RequestMapping("/api/v1/")
@CrossOrigin(origins = "http://localhost:3000")
public class ReservaControlador {

	@Autowired
	private ReservaServImplement servReserva;
	
	@PostMapping("/guardarReserva")
	public Reserva guardarReserva(@RequestBody Reserva reserva) {
		return servReserva.guardarReserva(reserva);
	}
}
