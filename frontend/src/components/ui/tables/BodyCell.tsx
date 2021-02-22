import * as React from "react"
import { PropsWithChildren } from "react"

const BodyCell: React.FunctionComponent = ({ children }: PropsWithChildren<unknown>): JSX.Element => {
	return (
		<td className="table__body-cell">
			{ children }
		</td>
	)
}

export default BodyCell