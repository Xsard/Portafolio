package com.turismo.backend_turismo_real.service;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import com.turismo.backend_turismo_real.modelo.Departamento;
import com.turismo.backend_turismo_real.repositorio.DepartamentoRepositorio;

@Service
public class DeptoServicioImplement implements DeptoServicio{
	@Autowired
	private DepartamentoRepositorio repodepto;
	
	@Override
	public List<Departamento> ObtenerDepto(){
		return repodepto.findAll();
	}
	
	@Override
	public ResponseEntity<Departamento> obtenerDeptoId(Integer id){
		Departamento depto = repodepto.findById(id)
				.orElseThrow();
		return ResponseEntity.ok(depto);
	}
	
	@Override
	public String QueryDepto(){
		return repodepto.QueryDepto();
	}
}