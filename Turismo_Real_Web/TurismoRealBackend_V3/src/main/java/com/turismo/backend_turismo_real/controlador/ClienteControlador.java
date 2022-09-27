package com.turismo.backend_turismo_real.controlador;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.turismo.backend_turismo_real.modelo.Cliente;
import com.turismo.backend_turismo_real.repositorio.ClienteRepositorio;
import com.turismo.backend_turismo_real.service.ClienteServicioImplement;

@RestController
@RequestMapping("/api/v1/")
@CrossOrigin(origins  = "http://localhost:3000")
public class ClienteControlador {
	
	@Autowired
	private ClienteServicioImplement serv;
	
	//obtener todos los clientes
	@GetMapping("/clientes")
	public void GetAllUser(){
		serv.GetAllUser();
	}
	@GetMapping("/putito")
	public String putito() {
		return serv.putito();
	}
	
	@GetMapping("/nose")
	public int nose() {
		return serv.nose();
	}
	
}
