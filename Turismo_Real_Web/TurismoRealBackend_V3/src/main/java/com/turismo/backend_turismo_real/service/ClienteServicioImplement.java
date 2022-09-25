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
	public List<Cliente> GetAllUser(){
		return repo.findAll();
	}
}
