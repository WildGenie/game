export type ApplicationUserModel = {
	id: string
	userName: string
	email: string
	isVerified: boolean
	roles: string[]
}

export type RegisterModel = {
	email: string
	userName: string
	password: string
	confirmPassword: string
	recaptchaToken: string
	acceptTos: boolean
}

export type ConfirmAccountModel = {
	userId: string
	verificationCode: string
}

export type LoginModel = {
	accountName: string
	password: string
	rememberMe: boolean
	recaptchaToken?: string
}

export enum LoginModelErrorKeys {
	accountName = "AccountName",
	recaptchaToken = "RecaptchaToken"
}

export type ForgotPasswordModel = {
	accountName: string
	recaptchaToken: string
}

export type ResetPasswordModel = {
	userId: string
	verificationCode: string
	newPassword: string
	confirmNewPassword: string
}

export enum ResetPasswordModelErrorKeys {
	UserId = "UserId",
	VerificationCode = "VerificationCode",
	NewPassword = "NewPassword",
	ConfirmNewPassword = "ConfirmNewPassword"
}

export type InitiateEmailChangeModel = {
	email: string
	confirmEmail: string
	confirmPassword: string
}

export type PerformEmailChangeModel = {
	newEmail: string
	userId: string
	verificationCode: string
}

export type InitiatePasswordChangeModel = {
	newPassword: string
	confirmNewPassword: string
	currentPassword: string
}

export enum InitiatePasswordChangeModelErrorKeys {
	NewPassword = "NewPassword",
	ConfirmNewPassword = "ConfirmNewPassword",
	CurrentPassword = "CurrentPassword"
}

export type DeleteAccountModel = {
	confirmPassword: string
}