import * as React from "react"
import { useState } from "react"

import Form from "@/components/ui/forms/Form"
import { OnSubmitValidator } from "@/tools/definitions/forms"
import { useFormInputValidation } from "@/hooks/forms"
import { required, requireMatch } from "@/tools/validations"
import TextField from "@/components/ui/forms/fields/TextField"
import { updateUserEmail } from "@/tools/browser/admin"
import { ApplicationUserModel } from "@/tools/definitions/users"
import { getDocumentTitle } from "@/tools/utils"

type EmailProps = {
	user: ApplicationUserModel
}

const Email: React.FunctionComponent<EmailProps> = ({ user }: EmailProps): JSX.Element => {

	const [wasSuccessful, setWasSuccessful] = useState(false)

	const email = useFormInputValidation<string>({
		initialValue: "",
		validators: [required]
	})
	const confirmEmail = useFormInputValidation<string>({
		initialValue: ""
	})

	confirmEmail.validators.push(() => requireMatch(email.value, confirmEmail.value, "The email addresses do not match"))

	const onSubmit: OnSubmitValidator = async () => {
		const response = await updateUserEmail({
			email: email.value,
			confirmEmail: confirmEmail.value,
			userId: user.id
		})

		if (!response.wasSuccessful) {
			email.setErrors(response.errors["Email"])
			confirmEmail.setErrors(response.errors["ConfirmEmail"])
		}

		setWasSuccessful(response.wasSuccessful)

		return response.wasSuccessful
	}

	document.title = getDocumentTitle(`Update ${user?.userName}'s email address`)

	return (
		<>
			{!wasSuccessful && (
				<Form
					validators={[email.validator, confirmEmail.validator]}
					handleSubmit={onSubmit}
					title={`${user?.userName}'s email`}
					showReset={false}
					showRecaptcha={false}
				>
					<p>Current email: {user?.email}</p>
					<TextField
						labelText="New email address"
						required={true}
						value={email.value}
						setValue={email.setValue}
						errors={email.errors}
						validator={email.validator}
					/>
					<TextField
						labelText="Confirm new email address"
						required={true}
						value={confirmEmail.value}
						setValue={confirmEmail.setValue}
						errors={confirmEmail.errors}
						validator={confirmEmail.validator}
					/>
				</Form>
			)}
			{wasSuccessful && (
				<p>
					User email address updated successfully.
				</p>
			)}
		</>
	)
}

export default Email