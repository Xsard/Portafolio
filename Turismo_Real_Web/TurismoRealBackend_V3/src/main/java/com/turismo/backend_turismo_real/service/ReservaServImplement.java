package com.turismo.backend_turismo_real.service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.turismo.backend_turismo_real.modelo.Reserva;
import com.turismo.backend_turismo_real.repositorio.ReservaRepositorio;

@Service
public class ReservaServImplement implements ReservaServicio{

	@Autowired
	private ReservaRepositorio reporeserva;
	
	@Override
	public Reserva guardarReserva(Reserva reserva) {
		return reporeserva.save(reserva);
	}

}
