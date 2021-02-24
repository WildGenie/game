import * as React from "react"
import { Dispatch, SetStateAction, useState } from "react"

import Form from "@/components/ui/forms/Form"
import { OnSubmitValidator } from "@/tools/definitions/forms"
import { useFormInputValidation } from "@/hooks/forms"
import { required, requireDigit, requireLength, requireLowerCase, requireMatch, requireSpecialChar, requireUpperCase } from "@/tools/validations"
import TextField from "@/components/ui/forms/fields/TextField"
import { ApplicationUserModel, InitiatePasswordChangeModelErrorKeys } from "@/tools/definitions/users"
import { updateUserPassword } from "@/tools/browser/admin"
import { getDocumentTitle } from "@/tools/utils"

type PasswordProps = {
	user: ApplicationUserModel
}

const Password: React.FunctionComponent<PasswordProps> = ({ user }: PasswordProps): JSX.Element => {

	const [wasSuccessful, setWasSuccessful] = useState(false)
	const [errors, setErrors]: [string[], Dispatch<SetStateAction<string[]>>] = useState([])

	const newPassword = useFormInputValidation<string>({
		initialValue: "",
		validators: [
			required,
			requireUpperCase,
			requireLowerCase,
			requireDigit,
			requireSpecialChar,
			(pw: string) => requireLength(pw, 8, 64)
		]
	})
	const confirmNewPassword = useFormInputValidation<string>({
		initialValue: ""
	})

	confirmNewPassword.validators.push(() => requireMatch(newPassword.value, confirmNewPassword.value, "The passwords do not match"))

	const onSubmit: OnSubmitValidator = async () => {
		const response = await updateUserPassword({
			userId: user.id,
			newPassword: newPassword.value,
			confirmNewPassword: confirmNewPassword.value,
		})

		if (!response.wasSuccessful) {
			newPassword.setErrors(response.errors[InitiatePasswordChangeModelErrorKeys.NewPassword])
			confirmNewPassword.setErrors(response.errors[InitiatePasswordChangeModelErrorKeys.ConfirmNewPassword])
			setErrors(response.errors["Unknown"])
		}

		setWasSuccessful(response.wasSuccessful)

		return response.wasSuccessful
	}

	document.title = getDocumentTitle(`Update ${user?.userName}'s password`)

	return (
		<>
			{!wasSuccessful && (
				<Form
					validators={[newPassword.validator, confirmNewPassword.validator]}
					handleSubmit={onSubmit}
					title={`${user?.userName}'s password`}
					showReset={false}
					showRecaptcha={false}
					showErrors={true}
					errors={errors}
				>
					<TextField
						labelText="New password"
						type="password"
						required={true}
						value={newPassword.value}
						setValue={newPassword.setValue}
						errors={newPassword.errors}
						validator={newPassword.validator}
					/>
					<TextField
						labelText="Confirm new password"
						type="password"
						required={true}
						value={confirmNewPassword.value}
						setValue={confirmNewPassword.setValue}
						errors={confirmNewPassword.errors}
						validator={confirmNewPassword.validator}
					/>
				</Form>
			)}
			{wasSuccessful && (
				<p>
					{user?.userName}&apos;s password has been updated successfully.
				</p>
			)}
		</>
	)
}

export default Password