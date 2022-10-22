package com.turismo.backend_turismo_real.service;

import java.util.Date;

import com.turismo.backend_turismo_real.modelo.Reserva;

public interface ReservaServicio {

	Reserva guardarReserva (Reserva reserva);
	
	int agregar_reserva (int id_depto, int id_cli, char estado_reserva, char estado_pago, Date check_in,
			Date check_out, int firma_res,int valor_total, int cantidad_acomp);
}
