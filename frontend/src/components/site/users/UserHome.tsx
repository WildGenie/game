import * as React from "react"

type UserHomeProps = {
	identifier: string
}

const UserHome: React.FunctionComponent<UserHomeProps> = ({ identifier }: UserHomeProps): JSX.Element => {
	return (
		<p>
			Please select an option to make changes to {identifier} account.
		</p>
	)
}

export default UserHome