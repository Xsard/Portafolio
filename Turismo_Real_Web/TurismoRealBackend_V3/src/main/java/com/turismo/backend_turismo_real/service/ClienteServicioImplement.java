package com.turismo.backend_turismo_real.service;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.turismo.backend_turismo_real.modelo.Cliente;
import com.turismo.backend_turismo_real.repositorio.ClienteRepositorio;

@Service
public class ClienteServicioImplement implements ClienteServicio{
	@Autowired
	private ClienteRepositorio repo;
	
	@Override
	public void GetAllUser(){
		//llamamos al procedure del repositorio
		repo.ListaProcedure();
	}
	
	@Override
	public String putito(){
		//llamamos al procedure del repositorio
		return repo.procedureName(1);
	}
	@Override
	public int nose(){
		//llamamos al procedure del repositorio
		return repo.nose();
	}
	
}
