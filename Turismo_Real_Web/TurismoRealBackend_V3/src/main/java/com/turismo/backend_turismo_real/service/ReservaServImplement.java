package com.turismo.backend_turismo_real.service;

import java.util.Date;

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

	@Override
	public int agregar_reserva(int id_depto, int id_cli, char estado_reserva, char estado_pago, Date check_in,
			Date check_out, int firma_res, int valor_total) {
		return reporeserva.agregar_reserva(id_depto, id_cli, estado_reserva, estado_pago, check_in, check_out, firma_res, valor_total);
	}

}
