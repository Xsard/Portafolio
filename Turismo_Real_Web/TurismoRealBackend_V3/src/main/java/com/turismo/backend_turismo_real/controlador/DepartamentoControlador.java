package com.turismo.backend_turismo_real.controlador;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.turismo.backend_turismo_real.modelo.Departamento;
import com.turismo.backend_turismo_real.service.DeptoServicioImplement;

@RestController
@RequestMapping("/api/v1/")
@CrossOrigin(origins = "http://localhost:3000")
public class DepartamentoControlador {
	
	@Autowired
	private DeptoServicioImplement deptoServ;

	@GetMapping("/deptosList")
	public List<Departamento> ObtenerDepto() {
		return deptoServ.ObtenerDepto();
	}
	@GetMapping("/depto/{id}")
	public ResponseEntity<Departamento> obtenerDeptoId(@PathVariable Integer id){
		return deptoServ.obtenerDeptoId(id);
	}
}
