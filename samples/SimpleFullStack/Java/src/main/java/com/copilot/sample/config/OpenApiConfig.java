package com.copilot.sample.config;

import io.swagger.v3.oas.models.OpenAPI;
import io.swagger.v3.oas.models.info.Contact;
import io.swagger.v3.oas.models.info.Info;
import io.swagger.v3.oas.models.info.License;
import io.swagger.v3.oas.models.servers.Server;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import java.util.List;

@Configuration
public class OpenApiConfig {    @Bean
    public OpenAPI customOpenAPI() {
        return new OpenAPI()
                .info(new Info()
                        .title("Copilot Sample API")
                        .description("A sample e-commerce inventory management API built with Spring Boot 3 and Java 21. " +
                                   "This API provides endpoints for managing products, categories, and product attributes.")
                        .version("1.0.0")
                        .contact(new Contact()
                                .name("Copilot Sample Team")
                                .email("support@copilotsample.com")
                                .url("https://github.com/copilot-sample"))
                        .license(new License()
                                .name("MIT License")
                                .url("https://opensource.org/licenses/MIT")))
                .servers(List.of(
                        new Server()
                                .url("http://localhost:5000")
                                .description("Development server"),
                        new Server()
                                .url("https://api.copilotsample.com")
                                .description("Production server")
                ));
    }
}
