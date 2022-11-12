package com.turismo.backend_turismo_real.controlador;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.turismo.backend_turismo_real.modelo.ServExtraReserva;
import com.turismo.backend_turismo_real.service.ServExtraReservaServImpl;


@RestController
@RequestMapping("/api/v1/")
@CrossOrigin(origins = "http://localhost:3000")
public class ServExtraReservaControlador {
	
	@Autowired
	private ServExtraReservaServImpl servExtraServ;
	
	@PostMapping("/addservExtra")
	public ServExtraReserva agregar_serv_extra(@RequestBody ServExtraReserva svExtra) {
		return servExtraServ.agregar_serv_extra(svExtra);
	}
}
