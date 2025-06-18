package com.copilot.sample.controller;

import com.copilot.sample.dto.ProductAttributeDto;
import com.copilot.sample.service.ProductAttributeService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/product-attributes")
@CrossOrigin(origins = "*")
@Tag(name = "Product Attributes", description = "Product attribute management APIs")
public class ProductAttributeController {    @Autowired
    private ProductAttributeService productAttributeService;

    @Operation(summary = "Get attributes by product ID", description = "Retrieve all attributes for a specific product")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Successfully retrieved product attributes",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ProductAttributeDto.class)))
    })
    @GetMapping("/product/{productId}")
    public ResponseEntity<List<ProductAttributeDto>> getAttributesByProductId(
            @Parameter(description = "ID of the product to get attributes for", required = true)
            @PathVariable Integer productId) {
        List<ProductAttributeDto> attributes = productAttributeService.getAttributesByProductId(productId);
        return ResponseEntity.ok(attributes);
    }

    @Operation(summary = "Get attribute by ID", description = "Retrieve a specific product attribute by its ID")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Attribute found",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ProductAttributeDto.class))),
            @ApiResponse(responseCode = "404", description = "Attribute not found", content = @Content)
    })
    @GetMapping("/{id}")
    public ResponseEntity<ProductAttributeDto> getAttributeById(
            @Parameter(description = "ID of the attribute to retrieve", required = true)
            @PathVariable Integer id) {
        ProductAttributeDto attribute = productAttributeService.getAttributeById(id);
        if (attribute != null) {
            return ResponseEntity.ok(attribute);
        }
        return ResponseEntity.notFound().build();
    }

    @Operation(summary = "Create a new product attribute", description = "Create a new product attribute in the system")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "201", description = "Attribute created successfully",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ProductAttributeDto.class))),
            @ApiResponse(responseCode = "400", description = "Invalid input data", content = @Content)
    })
    @PostMapping
    public ResponseEntity<ProductAttributeDto> createAttribute(
            @Parameter(description = "Attribute data to create", required = true)
            @Valid @RequestBody ProductAttributeDto productAttributeDto) {
        ProductAttributeDto createdAttribute = productAttributeService.createAttribute(productAttributeDto);
        return ResponseEntity.status(HttpStatus.CREATED).body(createdAttribute);
    }

    @Operation(summary = "Update a product attribute", description = "Update an existing product attribute by its ID")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Attribute updated successfully",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ProductAttributeDto.class))),
            @ApiResponse(responseCode = "404", description = "Attribute not found", content = @Content),
            @ApiResponse(responseCode = "400", description = "Invalid input data", content = @Content)
    })
    @PutMapping("/{id}")
    public ResponseEntity<ProductAttributeDto> updateAttribute(
            @Parameter(description = "ID of the attribute to update", required = true)
            @PathVariable Integer id,
            @Parameter(description = "Updated attribute data", required = true)
            @Valid @RequestBody ProductAttributeDto productAttributeDto) {
        ProductAttributeDto updatedAttribute = productAttributeService.updateAttribute(id, productAttributeDto);
        if (updatedAttribute != null) {
            return ResponseEntity.ok(updatedAttribute);
        }
        return ResponseEntity.notFound().build();
    }    @Operation(summary = "Delete a product attribute", description = "Delete a product attribute by its ID")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "204", description = "Attribute deleted successfully", content = @Content),
            @ApiResponse(responseCode = "404", description = "Attribute not found", content = @Content)
    })
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteAttribute(
            @Parameter(description = "ID of the attribute to delete", required = true)
            @PathVariable Integer id) {
        boolean deleted = productAttributeService.deleteAttribute(id);
        if (deleted) {
            return ResponseEntity.noContent().build();
        }
        return ResponseEntity.notFound().build();
    }
}
