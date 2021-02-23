import * as React from "react"
import { NavLink } from "react-router-dom"

const EditSuccessful: React.FunctionComponent = (): JSX.Element => {
	return (
		<p>
			Species was edited successfully! <NavLink to="/admin/species">Return to species list</NavLink>
		</p>
	)
}

export default EditSuccessful