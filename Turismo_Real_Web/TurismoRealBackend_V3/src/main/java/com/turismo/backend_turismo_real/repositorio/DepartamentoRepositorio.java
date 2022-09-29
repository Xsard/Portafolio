package com.turismo.backend_turismo_real.repositorio;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.turismo.backend_turismo_real.modelo.Departamento;

@Repository
public interface DepartamentoRepositorio extends JpaRepository<Departamento, Integer>{

}
