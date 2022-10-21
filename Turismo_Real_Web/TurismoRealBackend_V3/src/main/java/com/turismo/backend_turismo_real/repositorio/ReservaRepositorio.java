package com.turismo.backend_turismo_real.repositorio;

import java.util.Date;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.query.Procedure;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.turismo.backend_turismo_real.modelo.Reserva;

@Repository
public interface ReservaRepositorio extends JpaRepository<Reserva, Integer>{

	
	@Procedure(name="add_reserva")
	int agregar_reserva(@Param("idDepto") int id_depto, @Param("idCli") int id_cli,
			@Param("estadoRes") char estado_reserva, @Param("estadoPag") char estado_pago, 
			@Param("checkIn") Date check_in,@Param("checkOut") Date check_out ,@Param("firmaRes") int firma_res,
			@Param("valorTotal") int valor_total);
}
