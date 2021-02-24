import * as React from "react"
import { Dispatch, SetStateAction, useState } from "react"
import Form from "@/components/ui/forms/Form"
import { Roles as ApplicationRoles } from "@/tools/definitions/general"
import { ApplicationUserModel } from "@/tools/definitions/users"
import { useCheckInput } from "@/hooks/forms"
import CheckField from "@/components/ui/forms/fields/CheckField"
import { addUserToRole } from "@/tools/browser/admin"
import { getDocumentTitle } from "@/tools/utils"

type RolesProps = {
	user: ApplicationUserModel
}

const Roles: React.FunctionComponent<RolesProps> = ({ user }: RolesProps): JSX.Element => {

	const [errors, setErrors]: [string[], Dispatch<SetStateAction<string[]>>] = useState([])
	const [wasSuccessful, setWasSuccessful] = useState(false)
	const roles = [
		useCheckInput(ApplicationRoles.administrator, user.roles.includes(ApplicationRoles.administrator))
	]

	const onSubmit = async () => {
		const newRoles = roles
			.filter(r => r.checked)
			.map(r => r.value)

		const response = await addUserToRole({
			userId: user.id,
			roles: newRoles
		})

		if (!response.wasSuccessful) {
			setErrors(response.errors["Unknown"])
		}

		setWasSuccessful(response.wasSuccessful)

		return response.wasSuccessful
	}

	document.title = getDocumentTitle(`Update ${user?.userName}'s roles`)

	return (
		<>
			{!wasSuccessful && (
				<Form
					handleSubmit={onSubmit}
					title={`${user?.userName}'s roles`}
					showReset={false}
					showRecaptcha={false}
					showLegend={false}
					errors={errors}
				>
					{roles.map((role, i) => (
						<CheckField
							key={i}
							labelText={role.value}
							value={role.checked}
							setValue={role.setChecked}
						/>
					))}
				</Form>
			)}

			{wasSuccessful && (
				<p>
					{user.userName}&apos;s roles were updated successfully! You should reload the page to make sure that changes take effect on your page (reloading the page just refreshes your application state; {user.userName}&apos;s roles have already been updated either way).
				</p>
			)}
		</>
	)
}

export default Roles