import { makeRequest } from "@/tools/browser/browser"
import { ApiResponse } from "@/tools/definitions/requests"
import { ApplicationUserModel, ConfirmAccountModel, RegisterModel, LoginModel, LoginModelErrorKeys, ForgotPasswordModel, ResetPasswordModel, ResetPasswordModelErrorKeys, InitiateEmailChangeModel, InitiatePasswordChangeModel, InitiatePasswordChangeModelErrorKeys, PerformEmailChangeModel, DeleteAccountModel } from "@/tools/definitions/users"
import { processErrors } from "@/tools/utils"

export function register(data: RegisterModel): Promise<ApiResponse<ApplicationUserModel>> {
	return makeRequest("/account", "POST", data)
}

export async function confirm(data: ConfirmAccountModel): Promise<ApiResponse<boolean>> {
	const response = await makeRequest<ConfirmAccountModel, boolean>("/account/confirm", "POST", data)

	if (response.status === 404)
		response.errors["Unknown"] = [`An account with ID ${data.userId} was not found`]

	return response
}

export function checkLogin(): Promise<ApiResponse<ApplicationUserModel>> {
	return makeRequest("/account", "GET")
}

export async function login(data: LoginModel): Promise<ApiResponse<ApplicationUserModel>> {
	const response = await makeRequest<LoginModel, ApplicationUserModel>("/account/login", "POST", data)

	if (response.errors) {
		response.errors = processErrors(
			response.errors,
			LoginModelErrorKeys.accountName,
			LoginModelErrorKeys.recaptchaToken
		)
	}

	return response
}

export function logout(): Promise<ApiResponse<boolean>> {
	return makeRequest("/account/login", "DELETE")
}

export function forgotPassword(data: ForgotPasswordModel): Promise<ApiResponse<boolean>> {
	return makeRequest("/account/password", "DELETE", data)
}

export async function resetPassword(data: ResetPasswordModel): Promise<ApiResponse<boolean>> {
	const response = await makeRequest<ResetPasswordModel, boolean>("/account/password", "PATCH", data)

	if (response.errors) {
		response.errors = processErrors(
			response.errors,
			ResetPasswordModelErrorKeys.NewPassword,
			ResetPasswordModelErrorKeys.ConfirmNewPassword,
			"Unknown"
		)
	}

	if (response.status === 404) {
		response.errors = response.errors || {}
		response.errors["Unknown"] = response.errors["Unknown"] || []
		response.errors["Unknown"].push(`An account with ID ${data.userId} was not found`)
	}

	return response
}

export async function initiateEmailChange(data: InitiateEmailChangeModel): Promise<ApiResponse<unknown>> {
	const response = await makeRequest("/account/email", "POST", data)

	if (response.status === 401) {
		response.errors = response.errors || {}
		response.errors["ConfirmPassword"] = ["Your password was incorrect"]
	}

	return response
}

export async function performEmailChange(data: PerformEmailChangeModel): Promise<ApiResponse<unknown>> {
	const response = await makeRequest("/account/email", "PATCH", data)

	if (response.errors) {
		response.errors = processErrors(response.errors)
	}

	return response
}

export async function initiatePasswordChange(data: InitiatePasswordChangeModel): Promise<ApiResponse<unknown>> {
	const response = await makeRequest("/account/password", "POST", data)

	if (response.errors) {
		response.errors = processErrors(
			response.errors,
			InitiatePasswordChangeModelErrorKeys.NewPassword,
			InitiatePasswordChangeModelErrorKeys.ConfirmNewPassword,
			InitiatePasswordChangeModelErrorKeys.CurrentPassword
		)
	}

	return response
}

export async function deleteAccount(data: DeleteAccountModel): Promise<ApiResponse<unknown>> {
	const response = await makeRequest("/account", "DELETE", data)

	if (response.errors)
		response.errors = processErrors(response.errors)

	return response
}