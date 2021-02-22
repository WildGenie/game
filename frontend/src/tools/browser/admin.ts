import { ApiResponse } from "@/tools/definitions/requests"
import { UserAddToRoleModel, UserChangeEmailModel, UserChangePasswordModel, UserChangePasswordModelKeys, UserCountModel } from "@/tools/definitions/admin"
import { makeRequest } from "@/tools/browser/browser"
import { ApplicationUserModel } from "@/tools/definitions/users"
import { processErrors } from "@/tools/utils"

export function getUserCount(): Promise<ApiResponse<UserCountModel>> {
	return makeRequest("/users/count", "GET")
}

export function getUsersByPage(page = 1, resultsPerPage = 10): Promise<ApiResponse<ApplicationUserModel[]>> {
	return makeRequest(`/users/pagified?page=${page}&resultsPerPage=${resultsPerPage}`, "GET")
}

export function getUserData(userId: string): Promise<ApiResponse<ApplicationUserModel>> {
	return makeRequest(`/users?userId=${userId}`, "GET")
}

export async function updateUserEmail(data: UserChangeEmailModel): Promise<ApiResponse<unknown>> {
	const response = await makeRequest("/users/email", "PATCH", data)

	if (!response.wasSuccessful) {
		if (!response.errors)
			response.errors = {}
		response.errors = processErrors(response.errors)
	}

	if (response.status === 404) {
		response.errors["Unknown"] = response.errors["Unknown"] || []
		response.errors["Unknown"].push(`A user with id ${data.userId} was not found`)
	}

	return response
}

export async function updateUserPassword(data: UserChangePasswordModel): Promise<ApiResponse<unknown>> {
	const response = await makeRequest("/users/password", "PATCH", data)

	if (!response.wasSuccessful) {
		response.errors = processErrors(
			response.errors || {},
			UserChangePasswordModelKeys.UserId,
			UserChangePasswordModelKeys.Password,
			UserChangePasswordModelKeys.NewPassword,
			UserChangePasswordModelKeys.ConfirmNewPassword
		)

		if (response.errors["Password"]) {
			response.errors["NewPassword"] = response.errors["NewPassword"] || []
			response.errors["NewPassword"].push(...response.errors["Password"])
		}
	}

	if (response.status === 404) {
		response.errors["Unknown"] = response.errors["Unknown"] || []
		response.errors["Unknown"].push(`A user with id ${data.userId} was not found`)
	}

	return response
}

export async function addUserToRole(data: UserAddToRoleModel): Promise<ApiResponse<unknown>> {
	const response = await makeRequest("/users/roles", "POST", data)

	if (!response.wasSuccessful)
		response.errors = processErrors(response.errors || {})

	return response
}