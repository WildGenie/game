import { ApiResponse, HttpMethod } from "@/tools/definitions/requests"

export async function makeRequest<TPayload, TResponse>(url: string, method: HttpMethod, payload: TPayload = null): Promise<ApiResponse<TResponse>> {
	const requestInit: RequestInit = {
		method,
		mode: "same-origin",
		headers: {
			"Content-type": "application/json"
		}
	}
	if (payload)
		requestInit.body = JSON.stringify(payload)

	const request = new Request(`/api${url}`, requestInit)

	const response: ApiResponse<TResponse> = {
		result: null,
		errors: null,
		wasSuccessful: false,
		status: 500
	}

	let result
	try {
		result = await fetch(request)

		// If there is no body, result.json() will throw
		try {
			const json = await result.json()
			response.result = json["result"]
			response.errors = json["errors"]
		} catch (e) {
			response.result = null
			response.errors = null
		}

		response.wasSuccessful = result.status < 400
		response.status = result.status
	} catch (e) {
		// network errors only
		response.errors = {
			network: ["You don't appear to be connected to the internet. Please try again when your connection is reestablished."]
		}
		response.wasSuccessful = false
	}

	return response
}