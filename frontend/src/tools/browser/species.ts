import { AddSpeciesModel, EditSpeciesModel, SpeciesModel } from "@/tools/definitions/species"
import { ApiResponse } from "@/tools/definitions/requests"
import { makeRequest } from "@/tools/browser/browser"
import { processErrors } from "@/tools/utils"

export function getSpecies(): Promise<ApiResponse<SpeciesModel[]>> {
	return makeRequest("/species", "GET")
}

export function getSpeciesById(id: string): Promise<ApiResponse<SpeciesModel>> {
	return makeRequest(`/species/${id}`, "GET")
}

export async function addSpecies(data: AddSpeciesModel): Promise<ApiResponse<unknown>> {
	const response = await makeRequest("/species", "POST", data)

	if (!response.wasSuccessful)
		response.errors = processErrors(response.errors)

	return response
}

export async function editSpecies(data: EditSpeciesModel): Promise<ApiResponse<unknown>> {
	const response = await makeRequest("/species", "PATCH", data)

	if (!response.wasSuccessful)
		response.errors = processErrors(response.errors)

	return response
}