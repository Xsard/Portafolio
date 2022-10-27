package com.turismo.backend_turismo_real.repositorio;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Repository;

import com.turismo.backend_turismo_real.modelo.Reserva;
import com.turismo.backend_turismo_real.modelo.superReserva;

@Repository
public interface superReservaRepositorio extends JpaRepository<Reserva, Integer>{

	@Query(nativeQuery = true, value= "SELECT NOMBRE_DPTO, CHECK_IN, CHECK_OUT, ESTADO_PAGO, VALOR_TOTAL FROM RESERVA RES JOIN DEPARTAMENTO DPTO USING(ID_DPTO) WHERE ID_CLIENTE= :id_cliente")
	List<superReserva> reserva_cliente(@Param("id_cliente") Integer id_cliente);
}
