package com.turismo.backend_turismo_real.service;

import java.util.List;

import org.springframework.http.ResponseEntity;

import com.turismo.backend_turismo_real.modelo.Departamento;

public interface DeptoServicio {

	List<Departamento> ObtenerDepto();

	public ResponseEntity<Departamento> obtenerDeptoId(Integer id);
	
	String QueryDepto();
}
