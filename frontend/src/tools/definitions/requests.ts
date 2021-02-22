// Response from backend API
export type ApiResponse<T> = {
	result: T
	errors: Record<string, string[]>
	wasSuccessful: boolean
	status: number
}

export type HttpMethod = "GET" | "HEAD" | "POST" | "PUT" | "DELETE" | "CONNECT" | "OPTIONS" | "TRACE" | "PATCH"