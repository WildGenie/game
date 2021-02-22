export type UserCountModel = {
	count: number
}

export type UserChangeEmailModel = {
	userId: string,
	email: string,
	confirmEmail: string
}

export type UserChangePasswordModel = {
	userId: string,
	newPassword: string,
	confirmNewPassword: string
}

export enum UserChangePasswordModelKeys {
	UserId = "UserId",
	Password = "Password",
	NewPassword = "NewPassword",
	ConfirmNewPassword = "ConfirmNewPassword"
}

export type UserAddToRoleModel = {
	userId: string,
	roles: string[]
}