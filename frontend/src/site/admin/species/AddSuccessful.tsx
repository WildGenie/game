import * as React from "react"
import { NavLink } from "react-router-dom"

const AddSuccessful: React.FunctionComponent = (): JSX.Element => {
	return (
		<p>
			Species was added successfully! <NavLink to="/admin/species/add">Add another</NavLink>
		</p>
	)
}

export default AddSuccessful