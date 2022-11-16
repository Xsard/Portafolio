package com.turismo.backend_turismo_real.service;

import java.util.List;

import org.springframework.http.ResponseEntity;

import com.turismo.backend_turismo_real.modelo.Tours;

public interface ToursServicio {
	List<Tours> traerTours(int id_reserva);
}
